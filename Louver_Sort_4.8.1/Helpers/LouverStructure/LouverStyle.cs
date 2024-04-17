using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Navigation;

namespace Louver_Sort_4._8._1.Helpers.LouverStructure
{
    public class LouverStyle
    {
        public enum LouverStyles
        {
            XL,
            Standard
        }

        public static LouverStyles ConvertToStyle(bool isXL)
        {
            switch (isXL)
            {
                case (true):
                   return LouverStyles.XL;
                default:
                    return LouverStyles.Standard;
            }
        }
    }
}
