using APIModels.Models;
using PreFinal.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace PreFinal
{
    public static class FrameApp
    {
        public static Frame FrameObject;

        private static bool IsScanPageOpen = false;

        private static MainPage mainPage = null;
        public static ViewIndentoryPage viewInventory = null;
        private static InventorizationPage inventorizationPage = null;
        private static ScanPage scanPage = null;
        private static PrintPage printPage = null;
        private static AddPage addPage = null;

       

        public static void ClearAllPages()
        {
            mainPage = null;
            viewInventory = null;
            inventorizationPage = null;
            scanPage = null;
            IsScanPageOpen = false;
            printPage = null;
            addPage = null;
        }

        public static void NavigateMainPage()
        {
            if (mainPage == null)
            {
                mainPage = new MainPage();
            }
            FrameObject.Navigate(mainPage);
        }
        public static void NavigateInventorizationPage()
        {
            if (inventorizationPage == null)
            {
                inventorizationPage = new InventorizationPage();
            }
            if (IsScanPageOpen)
            {
                FrameObject.Navigate(scanPage);
            }
            else
            {

                FrameObject.Navigate(inventorizationPage);
                inventorizationPage.DateList.SelectedIndex = -1;
            }
        }
        public static void NavigateNewInventorizationPage()
        {
            inventorizationPage = new InventorizationPage();
            
            if (IsScanPageOpen)
            {
                FrameObject.Navigate(scanPage);
            }
            else
            {
                FrameObject.Navigate(inventorizationPage);
            }
        }
        public static void UpdateInventorizationPage()
        {
            inventorizationPage = new InventorizationPage();
        }
        public static void NavigateViewInventoryPage()
        {
            if (viewInventory == null)
            {
                viewInventory = new ViewIndentoryPage();
            }
            FrameObject.Navigate(viewInventory);
        }
        public static void NavigateScanPage()
        {
            if (scanPage == null)
            {
                scanPage = new ScanPage();
            }
            FrameObject.Navigate(scanPage);
        }
        public static void NavigateScanPage(int stocktakingId, List<Inventorys> oldInventory)
        {
            if (scanPage == null)
            {
                scanPage = new ScanPage(stocktakingId, oldInventory);
            }
            FrameObject.Navigate(scanPage);
        }
        public static void NavigatePrintPage()
        {
            if(printPage == null)
            {
                printPage = new PrintPage();
            }
            FrameObject.Navigate(printPage);
        }
        public static void NavigateNewPrintPage()
        {
            FrameObject.Navigate(new PrintPage());
        }
        public static void NavigateAddPage()
        {
            if (addPage == null)
            {
                addPage = new AddPage();
            }
            FrameObject.Navigate(addPage);
        }
        public static void OnIsScanPageOpen() => IsScanPageOpen = true;
        public static void OffIsScanPageOpen() 
        {
            IsScanPageOpen = false;
            scanPage = null;
        }
    }
}
