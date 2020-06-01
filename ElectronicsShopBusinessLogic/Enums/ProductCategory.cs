using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ElectronicsShopBusinessLogic.Enums
{
    public enum ProductCategory
    {
        [Description("Бытовая Техника")]
        БытоваяТехника = 0,

        [Description("Электроника")]
        Электроника = 1,

        [Description("Компьютеры")]
        Компьютеры = 2,

        [Description("Инструменты")]
        Инструменты = 3,

        [Description("Аксессуары")]
        Аксессуары = 4,

        [Description("Офисные Товары")]
        ОфисныеТовары = 5
    }
}
