﻿using System;
using Microsoft.Extensions.Options;
using Q42.HueApi;
using Q42.HueApi.ColorConverters;
using Q42.HueApi.ColorConverters.HSB;
using Q42.HueApi.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace PresenceLight.Core
{
    public interface IHueService
    {
        Task SetColor(string availability, string lightId);
        Task<string> RegisterBridge();
        Task<IEnumerable<Light>> CheckLights();
        Task<string> FindBridge();
    }
    public class HueService : IHueService
    {
        private readonly ConfigWrapper _options;
        private LocalHueClient _client;

        public HueService(IOptionsMonitor<ConfigWrapper> optionsAccessor)
        {
            _options = optionsAccessor.CurrentValue;
        }

        public HueService(ConfigWrapper options)
        {
            _options = options;
        }

        public async Task SetColor(string availability, string lightId)
        {
            _client = new LocalHueClient(_options.LightSettings.Hue.HueIpAddress);
            _client.Initialize(_options.LightSettings.Hue.HueApiKey);


            var command = new LightCommand();

            switch (availability)
            {
                case "Available":
                    command.SetColor(new RGBColor("#009933"));
                    break;
                case "Busy":
                    command.SetColor(new RGBColor("#ff3300"));
                    break;
                case "BeRightBack":
                    command.SetColor(new RGBColor("#ffff00"));
                    break;
                case "Away":
                    command.SetColor(new RGBColor("#ffff00"));
                    break;
                case "DoNotDisturb":
                    command.SetColor(new RGBColor("#B03CDE"));
                    break;
                case "Offline":
                    command.SetColor(new RGBColor("#FFFFFF"));
                    break;
                case "Off":
                    command.SetColor(new RGBColor("#FFFFFF"));
                    break;
                default:
                    command.SetColor(new RGBColor(availability));
                    break;
            }

            if (availability == "Off")
            {
                command.On = false;
                await _client.SendCommandAsync(command, new List<string> { lightId });
                return;
            }

            if (_options.LightSettings.UseDefaultBrightness)
            {
                if (_options.LightSettings.DefaultBrightness == 0)
                {
                    command.On = false;
                }
                else
                {
                    command.On = true;
                    command.Brightness = Convert.ToByte(((Convert.ToDouble(_options.LightSettings.DefaultBrightness) / 100) * 254));
                    command.TransitionTime = new TimeSpan(0);
                }
            }
            else
            {
                if (_options.LightSettings.Hue.HueBrightness == 0)
                {
                    command.On = false;
                }
                else
                {
                    command.On = true;
                    command.Brightness = Convert.ToByte(((Convert.ToDouble(_options.LightSettings.Hue.HueBrightness) / 100) * 254));
                    command.TransitionTime = new TimeSpan(0);
                }
            }

            await _client.SendCommandAsync(command, new List<string> { lightId });
        }

        //Need to wire up a way to do this without user intervention
        public async Task<string> RegisterBridge()
        {
            if (string.IsNullOrEmpty(_options.LightSettings.Hue.HueApiKey))
            {
                try
                {
                    _client = new LocalHueClient(_options.LightSettings.Hue.HueIpAddress);

                    //Make sure the user has pressed the button on the bridge before calling RegisterAsync
                    //It will throw an LinkButtonNotPressedException if the user did not press the button

                    return await _client.RegisterAsync("presence-light", "presence-light");
                }
                catch
                {
                    return String.Empty;
                }
            }
            return String.Empty;
        }

        public async Task<string> FindBridge()
        {
            try
            {
                IBridgeLocator locator = new HttpBridgeLocator(); //Or: LocalNetworkScanBridgeLocator, MdnsBridgeLocator, MUdpBasedBridgeLocator
                var bridges = await locator.LocateBridgesAsync(TimeSpan.FromSeconds(5));
                if (bridges.Count() > 0)
                {
                    return bridges.FirstOrDefault().IpAddress;
                }
            }
            catch
            {
                return String.Empty;
            }
            return String.Empty;
        }
        public async Task<IEnumerable<Light>> CheckLights()
        {
            if (_client == null)
            {
                _client = new LocalHueClient(_options.LightSettings.Hue.HueIpAddress);
                _client.Initialize(_options.LightSettings.Hue.HueApiKey);
            }
            var lights = await _client.GetLightsAsync();
            // if there are no lights, get some
            if (lights.Count() == 0)
            {
                await _client.SearchNewLightsAsync();
                Thread.Sleep(40000);
                lights = await _client.GetNewLightsAsync();
            }
            return lights;
        }
    }
}
