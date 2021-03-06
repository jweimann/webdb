﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using WebDB.Client.Prism.Core;

namespace WebDB.Client.Prism.Modules.Toolbar
{
    /// <summary>
    /// Interaction logic for ContentViewA.xaml
    /// </summary>
    public partial class ContentViewA : UserControl, IContentView
    {
        public ContentViewA()
        {
            InitializeComponent();

            //ViewModel = viewModel;
        }

        public IViewModel ViewModel
        {
            get
            {
                return (IContentViewViewModel)DataContext;
                //return (IViewModel)DataContext;
            }

            set
            {
                DataContext = value;
            }
        }
    }
}
