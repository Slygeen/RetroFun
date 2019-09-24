﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using RetroFun.Controls;
using System.Threading;
using Sulakore.Components;
using Sulakore.Communication;
using RetroFun.Subscribers;

namespace RetroFun.Pages
{
    [ToolboxItem(true)]
    [DesignerCategory("UserControl")]
    public partial class BuyFurniBruteforcer : ObservablePage, ISubscriber
    {
        public BuyFurniBruteforcer()
        {
            InitializeComponent();
            System.Windows.Forms.Form.CheckForIllegalCrossThreadCalls = false;


            Bind(PurchaseLoopCoolDown, "Value", nameof(SpeedTimer1));
            Bind(CataloguePageIDBox, "Value", nameof(PageIDInt1));
            Bind(CatalogueFurniIDBox, "Value", nameof(FurniIDint1));
        }



        private bool _EnableLoop;
        public bool EnableLoop
        {
            get => _EnableLoop;
            set
            {
                _EnableLoop = value;
                RaiseOnPropertyChanged();
            }
        }

        public bool PageIDBruteForcerEnabled1;
        public bool FurnIIDBruteforcerEnabled1;
        public bool GlobalBruteforcerEnabled1;






        private int _SpeedTimer1;
        public int SpeedTimer1
        {
            get => _SpeedTimer1;
            set
            {
                _SpeedTimer1 = value;
                RaiseOnPropertyChanged();
            }
        }

        private int _PageIDInt1;
        public int PageIDInt1
        {
            get => _PageIDInt1;
            set
            {
                _PageIDInt1 = value;
                RaiseOnPropertyChanged();
            }
        }

        private int _FurniIDint1;
        public int FurniIDint1
        {
            get => _FurniIDint1;
            set
            {
                _FurniIDint1 = value;
                RaiseOnPropertyChanged();
            }
        }

        private bool _isFurniPurchased;
        public bool isFurniPurchased
        {
            get => _isFurniPurchased;
            set
            {
                _isFurniPurchased = value;
                RaiseOnPropertyChanged();
            }
        }


        public void InPurchaseOk(DataInterceptedEventArgs e)
        {
            IsPurchasedFurni(true);
        }



        private void SendPurchaseBtn_Click(object sender, EventArgs e)
        {
            SendPacket(PageIDInt1, FurniIDint1);
        }

        private void CatalogueSButtonLoopToggle_Click(object sender, EventArgs e)
        {
            CheckLoopStatus();
        }

        private void CataloguePageIdBruteforcerbtx_Click(object sender, EventArgs e)
        {
            CheckPageIDBruteforcer();
        }

        private void CatalogueFurnIDBruteForcerbtx_Click(object sender, EventArgs e)
        {
            CheckFurniIDBruteforcer();
        }


        private void CatalogueBruteForceBtn_Click(object sender, EventArgs e)
        {
            CheckBruteForcer();
        }

        private void CheckLoopStatus()
        {
            if (EnableLoop)
            {
                EnableLoop = false;
                WriteToButton(CatalogueSButtonLoopToggle, "Buy On loop : Disabled");
            }
            else
            {
                EnableLoop = true;
                WriteToButton(CatalogueSButtonLoopToggle, "Buy On loop : Enabled");
                StartLoop();
            }
        }


        private void CheckFurniIDBruteforcer()
        {
            if (FurnIIDBruteforcerEnabled1)
            {
                FurnIIDBruteforcerEnabled1 = false;

                EnableButton(CatalogueBruteForceBtn, true);
                EnableButton(CataloguePageIdBruteforcerbtx, true);
                EnableNButton(CataloguePageIDBox, true);
                EnableNButton(CatalogueFurniIDBox, true);
                WriteToButton(CatalogueFurnIDBruteForcerbtx, "FurniID Bruteforcer : Disabled");
            }
            else
            {
                FurnIIDBruteforcerEnabled1 = true;

                EnableButton(CatalogueBruteForceBtn, false);
                EnableButton(CataloguePageIdBruteforcerbtx, false);
                EnableNButton(CataloguePageIDBox, false);
                EnableNButton(CatalogueFurniIDBox, false);
                WriteToButton(CatalogueFurnIDBruteForcerbtx, "FurniID Bruteforcer : Enabled");
                BruteForceFurniID();
            }
        }

        private void CheckPageIDBruteforcer()
        {
            if (PageIDBruteForcerEnabled1)
            {
                PageIDBruteForcerEnabled1 = false;

                EnableButton(CatalogueBruteForceBtn, true);
                EnableButton(CatalogueFurnIDBruteForcerbtx, true);
                EnableNButton(CataloguePageIDBox, true);
                EnableNButton(CatalogueFurniIDBox, true);
                WriteToButton(CataloguePageIdBruteforcerbtx, "PageID Bruteforcer : Disabled");
            }
            else
            {
                PageIDBruteForcerEnabled1 = true;

                EnableButton(CatalogueBruteForceBtn, false);
                EnableButton(CatalogueFurnIDBruteForcerbtx, false);
                EnableNButton(CataloguePageIDBox, false);
                EnableNButton(CatalogueFurniIDBox, false);
                WriteToButton(CataloguePageIdBruteforcerbtx, "PageID Bruteforcer : Enabled");
                BruteForcePageID();
            }
        }

        private void CheckBruteForcer()
        {
            if (GlobalBruteforcerEnabled1)
            {
                GlobalBruteforcerEnabled1 = false;

                EnableButton(CataloguePageIdBruteforcerbtx, true);
                EnableButton(CatalogueFurnIDBruteForcerbtx, true);
                EnableNButton(CataloguePageIDBox, true);
                EnableNButton(CatalogueFurniIDBox, true);
                WriteToButton(CatalogueBruteForceBtn, "BruteForcer : Disabled");
            }
            else
            {
                GlobalBruteforcerEnabled1 = true;

                EnableButton(CataloguePageIdBruteforcerbtx, false);
                EnableButton(CatalogueFurnIDBruteForcerbtx, false);
                EnableNButton(CataloguePageIDBox, false);
                EnableNButton(CatalogueFurniIDBox, false);
                WriteToButton(CatalogueBruteForceBtn, "BruteForcer : Enabled");
                GlobalBruteForcer();
            }
        }

        private void WriteToButton(SKoreButton button, string text)
        {
            Invoke((MethodInvoker)delegate
            {
                button.Text = text;
            });
        }


        private void EnableNButton(NumericUpDown button, bool enabled)
        {
            Invoke((MethodInvoker)delegate
            {
                button.Enabled = enabled;
            });
        }

        private void EnableButton(SKoreButton button, bool enabled)
        {
            Invoke((MethodInvoker)delegate
            {
                button.Enabled = enabled;
            });
        }


        public bool IsReceiving => true;


        public void OnOutDiceTrigger(DataInterceptedEventArgs e) { }
        public void OnOutUserWalk(DataInterceptedEventArgs e) { }




        private void SendPacket(int PageID, int FurniID)
        {
            Connection.SendToServerAsync(
            Out.CatalogBuyItem,
            PageID,
            FurniID,
            furnITextBox.Text
            );


        }


        private void StartLoop()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                if (EnableLoop)
                {
                    SendPacket(PageIDInt1, FurniIDint1);
                    Thread.Sleep(SpeedTimer1);
                    StartLoop();
                }
            }).Start();
        }


        private void IsPurchasedFurni(bool value)

        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
             isFurniPurchased = value;
            }).Start();
        }


        private void BruteForcePageID()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;
                if (PageIDBruteForcerEnabled1)
                {
                    if (PageIDInt1 == int.MaxValue)
                    {
                        PageIDBruteForcerEnabled1 = false;

                        EnableButton(CatalogueBruteForceBtn, true);
                        EnableButton(CatalogueFurnIDBruteForcerbtx, true);
                        EnableNButton(CataloguePageIDBox, true);
                        EnableNButton(CatalogueFurniIDBox, true);
                        WriteToButton(CataloguePageIdBruteforcerbtx, "PageID Bruteforcer : Disabled");
                        Thread.CurrentThread.Abort();
                        return;
                    }



                    SendPacket(PageIDInt1, FurniIDint1);

                    Thread.Sleep(30);


                    if (isFurniPurchased)
                    {
                        Connection.SendToClientAsync(In.RoomUserWhisper, 0, "[Bruteforcer]: Found valid PageID : " + PageIDInt1 + " with FurniID : " + FurniIDint1, 0, 34, 0, -1);
                        IsPurchasedFurni(false);
                    }

                    PageIDInt1++;
                    BruteForcePageID();
                }
            }).Start();
        }


        private void BruteForceFurniID()
        {
            new Thread(() =>
            {

                Thread.CurrentThread.IsBackground = true;

                if (FurnIIDBruteforcerEnabled1)
                {

                    if (FurniIDint1 == int.MaxValue)
                    {
                        FurnIIDBruteforcerEnabled1 = false;

                        EnableButton(CatalogueBruteForceBtn, true);
                        EnableButton(CataloguePageIdBruteforcerbtx, true);
                        EnableNButton(CataloguePageIDBox, true);
                        EnableNButton(CatalogueFurniIDBox, true);

                        WriteToButton(CatalogueFurnIDBruteForcerbtx, "FurniID Bruteforcer : Disabled");
                        Thread.CurrentThread.Abort();
                        return;
                    }
                    SendPacket(PageIDInt1, FurniIDint1);
                    Thread.Sleep(30);


                    if (isFurniPurchased)
                    {
                        Connection.SendToClientAsync(In.RoomUserWhisper, 0, "[Bruteforcer]: Found valid PageID : " + PageIDInt1 + " with FurniID : " + FurniIDint1, 0, 34, 0, -1);
                        IsPurchasedFurni(false);
                    }

                    FurniIDint1++;

                    BruteForceFurniID();

                }

            }).Start();
        }


        private void GlobalBruteForcer()
        {
            new Thread(() =>
            {
                Thread.CurrentThread.IsBackground = true;

                if (GlobalBruteforcerEnabled1)
                {

                    if (FurniIDint1 == int.MaxValue)
                    {
                        FurniIDint1 = 0;
                        PageIDInt1++;
                    }


                    if (PageIDInt1 == int.MaxValue)
                    {
                        if (FurniIDint1 == int.MaxValue)
                        {

                            GlobalBruteforcerEnabled1 = false;

                            EnableButton(CataloguePageIdBruteforcerbtx, true);
                            EnableButton(CatalogueFurnIDBruteForcerbtx, true);
                            EnableNButton(CataloguePageIDBox, true);
                            EnableNButton(CatalogueFurniIDBox, true);
                            WriteToButton(CatalogueBruteForceBtn, "BruteForcer : Disabled");
                            Thread.CurrentThread.Abort();
                            return;
                        }
                    }

                    FurniIDint1++;

                    SendPacket(PageIDInt1, FurniIDint1);
                    Thread.Sleep(30);

                    if (isFurniPurchased)
                    {
                        Connection.SendToClientAsync(In.RoomUserWhisper, 0, "[Bruteforcer]: Found valid PageID : " + PageIDInt1 + " with FurniID : " + FurniIDint1, 0, 34, 0, -1);
                        IsPurchasedFurni(false);
                    }

                    GlobalBruteForcer();
                }


            }).Start();
        }
    }
}
