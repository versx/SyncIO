﻿namespace ScreenCaptureServerPlugin
{
    using SyncIO.Network;
    using SyncIO.ServerPlugin;
    using SyncIO.Transport.Packets;

    using ScreenCaptureServerPlugin.UI.Forms;

    public class ServerPlugin : ISyncIOServerPlugin, IUICallbacks
    {
        private readonly IUIHost _uiHost;
        private readonly INetHost _netHost;
        private readonly ILoggingHost _loggingHost;

        public string Name => "Screen Capture";

        public string Author => "versx";

        public ServerPlugin(IUIHost uiHost, INetHost netHost, ILoggingHost loggingHost)
        {
            _uiHost = uiHost;
            _netHost = netHost;
            _loggingHost = loggingHost;
        }

        public void OnPluginReady()
        {
            _loggingHost.Trace($"OnPluginReady");
            var contextEntry = new ContextEntry
            {
                Name = "Screen Capture",
                OnClick = (sender, e) =>
                {
                    foreach (var client in e)
                    {
                        var desktop = new RemoteDesktop(_netHost, client.Value)
                        {
                            Tag = client.Key
                        };
                        desktop.Show();
                    }
                }
            };
            _uiHost.AddContextMenuEntry(contextEntry);
            _loggingHost.Debug($"Added context entry {contextEntry.Name}...");
        }

        public void OnClientConnect(ISyncIOClient client)
        {
            _loggingHost.Trace($"OnClientConnect [Client={client.EndPoint}]");
        }

        public void OnClientDisconnect(ISyncIOClient client)
        {
            _loggingHost.Trace($"OnClientDisconnect [Client={client.EndPoint}]");
        }

        public void OnPacketReceived(ISyncIOClient client, IPacket packet)
        {
            _loggingHost.Trace($"OnPacketReceived [Client={client.EndPoint}, Packet={packet}]");
        }

        public void OnInvalidated()
        {
            _loggingHost.Trace($"OnInvalidated");
        }
    }
}