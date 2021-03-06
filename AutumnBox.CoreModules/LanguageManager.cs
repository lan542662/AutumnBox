﻿/*************************************************
** auth： zsh2401@163.com
** date:  2018/9/19 17:14:28 (UTC +8:00)
** desc： ...
*************************************************/
using AutumnBox.OpenFramework.Content;
using System.Collections.Generic;
using System.Windows;

namespace AutumnBox.CoreModules
{
    /// <summary>
    /// 语言管理器实现
    /// </summary>
    public class LanguageManager
    {
        private ResourceDictionary defaultLanguage;
        private readonly Dictionary<string, ResourceDictionary> Languages;
        private readonly Context ctx;
        /// <summary>
        /// 构造
        /// </summary>
        public LanguageManager(Context ctx)
        {
            Languages = new Dictionary<string, ResourceDictionary>();
            this.ctx = ctx;
        }
        /// <summary>
        /// 获取
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string Get(string key)
        {
            object result = null;
            ctx.App.RunOnUIThread(() =>
            {
                try
                {
                    result = Select(key);
                }
                catch { }
            });
            return result as string;
        }
        private string Select(string key)
        {
            var crtCode = ctx.App.CurrentLanguageCode.ToLower();
            if (Languages.TryGetValue(crtCode, out ResourceDictionary lang))
            {
                return lang[key] as string;
            }
            return defaultLanguage[key] as string;
        }
        /// <summary>
        /// 加载语言
        /// </summary>
        /// <param name="regionCode"></param>
        /// <param name="langResDict"></param>
        public void Load(string regionCode, ResourceDictionary langResDict)
        {
            if (defaultLanguage == null)
            {
                defaultLanguage = langResDict;
            }
            Languages[regionCode.ToLower()] = langResDict;
        }
    }
}
