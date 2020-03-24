﻿// Singleton.cs, 23.03.2020
// Copyright (C) Dominic Beger 24.03.2020

using System;
using System.Linq;
using System.Reflection;

namespace nUpdate.Administration.Infrastructure
{
    public class Singleton<T> where T : class
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                ManageInstance();
                return _instance;
            }
        }

        private static void ManageInstance()
        {
            if (_instance != null)
                return;

            var instanceType = typeof(T);
            if (instanceType.GetConstructors(BindingFlags.Public).Any())
                throw new InvalidOperationException(
                    $"Cannot create a single instance of {instanceType.Name} because it has at least one public constructor.");

            _instance = (T) Activator.CreateInstance(instanceType, true);
        }
    }
}