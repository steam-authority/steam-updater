﻿using static SteamKit2.SteamApps;

namespace SteamUpdater.Consumers.Messages
{
    public class PackageDataMessage
    {
        public PICSProductInfoCallback.PICSProductInfo PICSPackageInfo { get; set; }
    }
}