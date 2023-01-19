using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using CitizenFX.Core;
using CitizenFX.Core.Native;
using FivePD.API;
using FivePD.API.Utils;

namespace SirGerbain_VangelicoRobbery
{
    [CalloutProperties("Vangelico Robbery", "sirGerbain", "1.0.0")]
    public class SirGerbain_VangelicoRobbery : FivePD.API.Callout
    {

        List<VehicleHash> carList = new List<VehicleHash>();
        List<PedHash> robberHashList = new List<PedHash>();
        List<Ped> robberList = new List<Ped>();
        Vector3 robberyLocation = new Vector3(-628.49f,-235.32f, 38.06f);
        Vector3 robberWhiteCarEntrance = new Vector3(-638.07f, -236.7f,37.94f);
        Vector3 robberRedCarEntrance = new Vector3(-638.92f, -239.52f, 38.03f);
        Vector3 robberBlueCarEntrance = new Vector3(-639.94f, -243.39f, 38.02f);
        Random random = new Random();
        Vehicle whiteHeroCar, redHeroCar, blueHeroCar;
        bool arrivalOnScene = false;

        public SirGerbain_VangelicoRobbery()
        {

            robberHashList.Add(PedHash.SalvaGoon01GMY);
            robberHashList.Add(PedHash.SalvaGoon02GMY);
            robberHashList.Add(PedHash.SalvaBoss01GMY);

            InitInfo(robberyLocation);
            ShortName = "Vangelico Robbery in progress";
            CalloutDescription = "Vangelico Robbery";
            ResponseCode = 3;
            StartDistance = 150f;

        }

        public async override Task OnAccept()
        {
            InitBlip();
            UpdateData();

        }

        public async override void OnStart(Ped closest)
        {
            await setupCallout();
            base.OnStart(closest);

            while (!arrivalOnScene)
            {
                await BaseScript.Delay(500);
                float distance = Game.PlayerPed.Position.DistanceToSquared(robberyLocation);
                if (distance < 300f)
                {

                    using (TaskSequence whiteCarSequence = new TaskSequence())
                    {
                        whiteCarSequence.AddTask.ClearAll();
                        whiteCarSequence.AddTask.RunTo(new Vector3(-632.27f,-239.5f,38.1f));
                        whiteCarSequence.AddTask.RunTo(robberWhiteCarEntrance);
                        whiteCarSequence.AddTask.EnterVehicle(whiteHeroCar, VehicleSeat.Driver);
                        whiteCarSequence.Close();
                        robberList[0].Task.PerformSequence(whiteCarSequence);
                    }

                    using (TaskSequence redCarSequence = new TaskSequence())
                    {
                        redCarSequence.AddTask.ClearAll();  
                        redCarSequence.AddTask.RunTo(new Vector3(-632.27f, -239.5f, 38.1f));
                        redCarSequence.AddTask.RunTo(robberRedCarEntrance);
                        redCarSequence.AddTask.EnterVehicle(redHeroCar, VehicleSeat.Driver);
                        redCarSequence.Close();
                        robberList[1].Task.PerformSequence(redCarSequence);
                    }

                    using (TaskSequence blueCarSequence = new TaskSequence())
                    {
                        blueCarSequence.AddTask.ClearAll(); 
                        blueCarSequence.AddTask.RunTo(new Vector3(-632.27f, -239.5f, 38.1f));
                        blueCarSequence.AddTask.RunTo(robberBlueCarEntrance);
                        blueCarSequence.AddTask.EnterVehicle(blueHeroCar, VehicleSeat.Driver);
                        blueCarSequence.Close();
                        robberList[2].Task.PerformSequence(blueCarSequence);
                    }

                    Notify("THEY ARE LEAVING");
                    arrivalOnScene = true;  
                    break;
                }
            }
        }

        public async Task setupCallout()
        {
            for (int i = 0; i < 3; i++)
            {
                float offsetX = 2.0f * (float)Math.Cos(i * 120.0f * (Math.PI / 180.0));
                float offsetY = 2.0f * (float)Math.Sin(i * 120.0f * (Math.PI / 180.0));
                Vector3 robberLocation = robberyLocation + new Vector3(offsetX, offsetY, 0);
                Ped robber = await SpawnPed(robberHashList[random.Next(robberHashList.Count)], robberLocation);
                robber.AlwaysKeepTask = true;
                robber.BlockPermanentEvents = true;
                robber.Weapons.Give(WeaponHash.Pistol, 250, true, true);
                robber.Heading = random.Next(100, 150);
                robber.Task.GuardCurrentPosition();
                robberList.Add(robber);
            }

            whiteHeroCar = await SpawnVehicle(VehicleHash.Issi2, new Vector3(-636.3f,-236.91f,37.61f));
            whiteHeroCar.Heading = 38.51f;
            whiteHeroCar.Mods.PrimaryColor = VehicleColor.MetallicWhite;
            whiteHeroCar.Mods.SecondaryColor = VehicleColor.MetallicBlack;
            whiteHeroCar.Mods.PearlescentColor = VehicleColor.MetallicBlack;
            whiteHeroCar.IsEngineRunning = true;
            whiteHeroCar.Mods.LicensePlate = "ADAM";
            whiteHeroCar.Mods.InstallModKit();
            whiteHeroCar.Mods[VehicleModType.Engine].Index = 5;
            whiteHeroCar.Mods[VehicleModType.Brakes].Index = 4;
            whiteHeroCar.Mods[VehicleModType.Transmission].Index = 4;
            whiteHeroCar.Mods[VehicleModType.Transmission].Index = 4;
            whiteHeroCar.Mods[VehicleToggleModType.Turbo].IsInstalled = true;

            redHeroCar = await SpawnVehicle(VehicleHash.Issi2, new Vector3(-637.16f, -239.97f, 37.75f));
            redHeroCar.Heading = 40.07f;
            redHeroCar.Mods.PrimaryColor = VehicleColor.MetallicCandyRed;
            redHeroCar.Mods.SecondaryColor = VehicleColor.MetallicBlack;
            redHeroCar.Mods.PearlescentColor = VehicleColor.MetallicBlack;
            redHeroCar.IsEngineRunning = true;
            redHeroCar.Mods.LicensePlate = "ANDY";
            redHeroCar.Mods[VehicleModType.Engine].Index = 5;
            redHeroCar.Mods[VehicleModType.Brakes].Index = 4;
            redHeroCar.Mods[VehicleModType.Transmission].Index = 4;
            redHeroCar.Mods[VehicleModType.Transmission].Index = 4;
            redHeroCar.Mods[VehicleToggleModType.Turbo].IsInstalled = true;

            blueHeroCar = await SpawnVehicle(VehicleHash.Issi2, new Vector3(-637.96f, -243.81f, 37.81f));
            blueHeroCar.Heading = 36.82f;
            blueHeroCar.Mods.PrimaryColor = VehicleColor.MetallicBlue;
            blueHeroCar.Mods.SecondaryColor = VehicleColor.MetallicBlack;
            blueHeroCar.Mods.PearlescentColor = VehicleColor.MetallicBlack;
            blueHeroCar.IsEngineRunning = true;
            blueHeroCar.Mods.LicensePlate = "JASON";
            blueHeroCar.Mods[VehicleModType.Engine].Index = 5;
            blueHeroCar.Mods[VehicleModType.Brakes].Index = 4;
            blueHeroCar.Mods[VehicleModType.Transmission].Index = 4;
            blueHeroCar.Mods[VehicleModType.Transmission].Index = 4;
            blueHeroCar.Mods[VehicleToggleModType.Turbo].IsInstalled = true;

        }
        private void Notify(string message)
        {
            ShowNetworkedNotification(message, "CHAR_CALL911", "CHAR_CALL911", "Dispatch", "LSPD", 15f);
        }
        private void DrawSubtitle(string message, int duration)
        {
            API.BeginTextCommandPrint("STRING");
            API.AddTextComponentSubstringPlayerName(message);
            API.EndTextCommandPrint(duration, false);
        }

    }
}