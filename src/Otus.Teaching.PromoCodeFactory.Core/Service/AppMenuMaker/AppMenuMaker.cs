using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Otus.Teaching.PromoCodeFactory.Core
{
   public class AppMenuMaker
    {
        //TODO сделать через интерфейс способы вывода
        //TODO сделать словарь сообщений, возможно отдельным проектом

        //класс, который делает меню и запускает цикл его исполнения

        private IWritingDevice _writingDevice;

        private List<MenuItem> Items = new List<MenuItem>();
        private string MenuHeader="";
        private string MenuItemSeparator = "----------------------------";
        public delegate void MenuItemDelegate();
        private string MenuExitText = "";

        public void Run()
        {
            ShowMenu();
        }
        public AppMenuMaker(string menuHeader = "", string menuExitText = "")
        {
            MenuHeader = menuHeader;
            MenuExitText = menuExitText;
        }

        public AppMenuMaker(string menuHeader = "", string menuExitText = "", string menuItemSeparator="")
        {
            MenuItemSeparator = menuItemSeparator;
            MenuHeader = menuHeader;
            MenuExitText = menuExitText;
        }
        
        public void SetWritingDevice(IWritingDevice writingDevice)
        {
            _writingDevice = writingDevice;
        }

        public void AddMenuItemDelegate(int userNumber, string userText, MenuItemDelegate myDelegate)
        {
            MenuItem item = new MenuItem
            {
                MenuItemType = MenuItemTypeEnum.Acion,
                UserNumber = userNumber,
                UserText = userText, 
                MyDelegate = myDelegate
            };
            Items.Add(item);
        }
        public void AddMenuItemSubMenu(int userNumber, string userText, AppMenuMaker subMenu)
        {
            MenuItem item = new MenuItem
            {
                MenuItemType = MenuItemTypeEnum.SubMenu,
                UserNumber = userNumber,
                UserText = userText,
                 SubMenu = subMenu
            };
            subMenu.SetWritingDevice(_writingDevice);
            Items.Add(item);
        }

        public void AddSeparator()
        {
            MenuItem item = new MenuItem
            {
                MenuItemType = MenuItemTypeEnum.Separartor,
                UserNumber = -1,
                UserText = MenuItemSeparator
            };
            Items.Add(item);
        }

        private class MenuItem
        {
            public int UserNumber { get; set; } //какую цифру надо нажать пользователю, чтобы отработал этот пункт меню
            public string UserText { get; set; }
            public AppMenuMaker SubMenu { get; set; }
            public MenuItemTypeEnum MenuItemType { get; set; }
            public MenuItemDelegate MyDelegate { get; set; }

            public void Show()
            {
                Console.WriteLine($"{UserNumber}.  {UserText}");
            }
            public void Run()
            {
               switch(MenuItemType)
                {
                    case MenuItemTypeEnum.Acion:
                        MyDelegate();
                        break;
                    case MenuItemTypeEnum.SubMenu:
                        SubMenu.Run();
                        break;
                }
                
            }

        }

        private void  ShowMenu()
        {
            
            while(true)
            {
                _writingDevice.WriteLine("");
                _writingDevice.WriteLine($"{MenuHeader}");
                _writingDevice.WriteLine("");
                _writingDevice.WriteLine($"Выберите опцию из меню ниже (введите число), или '/' для выхода ");

                Items.ForEach(x => x.Show());

                int targetNumber = -1;

                string myInput = Console.ReadLine();

                if (myInput == @"/") break;

                if (!int.TryParse(myInput, out targetNumber))
                {
                    _writingDevice.WriteRed("Введите корректный номер пункта меню, или '/' для выхода");
                    continue;
                };

                if (!hasMenuItem(targetNumber))
                {
                    _writingDevice.WriteRed("Введите корректный номер пункта меню, или '/' для выхода");
                    continue;
                };

                //запустить пункт меню
                MenuItem menuItem = getMenuItemByMumberOrNull(targetNumber);

                if (menuItem==null)
                {
                    _writingDevice.WriteRed("Неизвестная ошибка, попробуйте снова");
                    continue;
                }
                
                menuItem.Run();
            }
        }
        private bool hasMenuItem(int number)
        {
            var rez = Items.Where(x => x.UserNumber == number).ToList();
            return rez.Count > 0;
        }

        private MenuItem getMenuItemByMumberOrNull(int number)
        {
            var rez = Items.Where(x => x.UserNumber == number).ToList();

            if (rez.Count == 0) return null; else return rez.FirstOrDefault();
        }


        private enum MenuItemTypeEnum
        {
            Acion=1,
            SubMenu=2,
            Separartor = 3
        }

        public interface IWritingDevice
        {
            public void WriteWithColor(string text, ConsoleColor color);
            public void WriteDefault(string text);
            public void WriteRed(string text);
            public void WriteGreen(string text);
            public void WriteYellow(string text);
            public void WriteLine(string text);

        }
    }


}
