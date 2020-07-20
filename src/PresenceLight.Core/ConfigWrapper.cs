﻿using System;
using System.ComponentModel.DataAnnotations;

using Microsoft.Graph;

namespace PresenceLight.Core
{
    public class ConfigWrapper
    {
        public string? IconType { get; set; }
        public LightSettings LightSettings { get; set; }

        public string? ClientId { get; set; }

        public string? ClientSecret { get; set; }

        public string? Instance { get; set; }

        public string? RedirectUri { get; set; }

        public string? CallbackPath { get; set; }

        public string? ApiScopes { get; set; }
    }

    public class AADSettings
    {
        public string? ClientId { get; set; }

        public string? ClientSecret { get; set; }

        public string? Instance { get; set; }

        public string? RedirectUri { get; set; }

        public string? CallbackPath { get; set; }

        public string? ApiScopes { get; set; }
    }

    public class LightSettings
    {
        public bool SyncLights { get; set; }

        public string WorkingDays { get; set; }

        public bool UseWorkingHours { get; set; }

        public string WorkingHoursStartTime { get; set; }

        public string WorkingHoursEndTime { get; set; }

        public double PollingInterval { get; set; }

        public bool UseDefaultBrightness { get; set; }
        public int DefaultBrightness { get; set; }
        public Custom Custom { get; set; }

        public LIFX LIFX { get; set; }

        public Hue Hue { get; set; }

        public Yeelight Yeelight { get; set; }
    }


    public class Custom
    {
        public bool IsCustomApiEnabled { get; set; }

        public string? SelectedCustomLightId { get; set; }

        public string? CustomApiAvailableMethod { get; set; }

        public string? CustomApiAvailableUri { get; set; }

        public string? CustomApiBusyMethod { get; set; }

        public string? CustomApiBusyUri { get; set; }

        public string? CustomApiBeRightBackMethod { get; set; }

        public string? CustomApiBeRightBackUri { get; set; }

        public string? CustomApiAwayMethod { get; set; }

        public string? CustomApiAwayUri { get; set; }

        public string? CustomApiDoNotDisturbMethod { get; set; }

        public string? CustomApiDoNotDisturbUri { get; set; }

        public string? CustomApiOfflineMethod { get; set; }

        public string? CustomApiOfflineUri { get; set; }

        public string? CustomApiOffMethod { get; set; }

        public string? CustomApiOffUri { get; set; }

        public double CustomApiTimeout { get; set; }
    }

    public class LIFX
    {

        public string? LIFXApiKey { get; set; }

        public string? LIFXClientId { get; set; }

        public string? LIFXClientSecret { get; set; }

        public bool IsLIFXEnabled { get; set; }

        public string? SelectedLIFXItemId { get; set; }

        public int LIFXBrightness { get; set; }
    }

    public class Hue
    {
        public string? HueApiKey { get; set; }

        public string? SelectedHueLightId { get; set; }

        public int HueBrightness { get; set; }

        [Required]
        [RegularExpression(@"\b\d{1,3}\.\d{1,3}\.\d{1,3}\.\d{1,3}\b",
        ErrorMessage = "Not a valid IP Address")]
        public string? HueIpAddress { get; set; }

        public bool IsPhillipsHueEnabled { get; set; }
    }

    public class Yeelight
    {
        public string? SelectedYeelightId { get; set; }

        public int YeelightBrightness { get; set; }

        public bool IsYeelightEnabled { get; set; }
    }
}
