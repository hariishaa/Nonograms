﻿using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using Microsoft.Practices.ServiceLocation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Nonograms.Portable.ViewModel
{
    public class BaseViewModelLocator
    {
        public BaseViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);
            SimpleIoc.Default.Register<LevelsPageViewModel>();
            SimpleIoc.Default.Register<NonogramPageViewModel>();
        }

        public LevelsPageViewModel LevelsPage => ServiceLocator.Current.GetInstance<LevelsPageViewModel>();
        public NonogramPageViewModel NonogramPage => ServiceLocator.Current.GetInstance<NonogramPageViewModel>();
    }
}
