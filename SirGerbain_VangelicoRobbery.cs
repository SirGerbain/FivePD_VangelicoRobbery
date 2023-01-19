using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;
using FivePD.API.Utils;

namespace SirGerbain_VangelicoRobbery
{
    [CalloutProperties("PDM Robbery", "sirGerbain", "1.0.0")]
    public class SirGerbain_VangelicoRobbery : FivePD.API.Callout
    {


        public SirGerbain_VangelicoRobbery()
        {

            InitInfo();
            ShortName = "PDM Robbery in progress";
            CalloutDescription = "PDM Robbery";
            ResponseCode = 3;
            StartDistance = 200f;

        }

        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();

        }

        public async override void OnStart(Ped closest)
        {
            setupCallout();
            base.OnStart(closest);

        }

        public async void setupCallout()
        {
   

        }
        private void Notify(string message)
        {
            ShowNetworkedNotification(message, "CHAR_CALL911", "CHAR_CALL911", "Dispatch", "AIR-1", 15f);
        }
        private void DrawSubtitle(string message, int duration)
        {
            API.BeginTextCommandPrint("STRING");
            API.AddTextComponentSubstringPlayerName(message);
            API.EndTextCommandPrint(duration, false);
        }

    }
}