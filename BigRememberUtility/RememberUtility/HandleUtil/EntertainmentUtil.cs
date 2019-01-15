﻿using System;
using System.Collections.Generic;
using System.Linq;
using RememberUtility.Enum;
using RememberUtility.Extension;
using RememberUtility.Interface;
using RememberUtility.Model;
using log4net;
using System.Text;

namespace RememberUtility.HandleUtil
{
    public class EntertainmentUtil : IEntertainment
    {
        private readonly FileHandlerUtil _fileHandlerUtil;
        private static readonly ILog Logs = LogManager.GetLogger(typeof(EntertainmentUtil));

        public EntertainmentUtil()
        {
            _fileHandlerUtil = new FileHandlerUtil();
            _fileHandlerUtil.CreateOrReadJsonDb(EnumFileConstant.ENTERTAINMENTCONSTAT);            
        }

        public void AddEntertainment(Entertainment et)
        {
            et.CreatedDate = $"{DateTime.Now:MMMM dd, yyyy}";
            et.EnterId = HandleRandom.RandomString(8);
            _fileHandlerUtil.JsonModel.Entertainment.Add(et);
            Logs.Info($"[AddEntertainment] Adding '{et.EnterName}' successful.");

            _fileHandlerUtil.SaveFile(EnumFileConstant.ENTERTAINMENTCONSTAT);
        }

        public Entertainment FindEntertainmentBy(string enterName)
        {
            try
            {
                _fileHandlerUtil.SaveFile(EnumFileConstant.ENTERTAINMENTCONSTAT);

                return _fileHandlerUtil.JsonModel.Entertainment.Find(e =>
                string.Equals(e.EnterName, enterName, StringComparison.CurrentCultureIgnoreCase));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public Entertainment FindEntertainmentByEnterId(string enterId)
        {
            try
            {
                _fileHandlerUtil.SaveFile(EnumFileConstant.ENTERTAINMENTCONSTAT);

                return _fileHandlerUtil.JsonModel.Entertainment.Find(x =>
                    string.Equals(x.EnterId, enterId, StringComparison.CurrentCultureIgnoreCase));
            }
            catch (Exception)
            {
                return null;
            }
        }

        public bool UpdateEntertainment(string currentEnterName, string enterNewName, string newLink, string newCategory)
        {
            var getCurrentEt = _fileHandlerUtil.JsonModel.Entertainment.
                Find(x => string.Equals(x.EnterName, currentEnterName, StringComparison.CurrentCultureIgnoreCase));
            var indexOfEt = _fileHandlerUtil.JsonModel.Entertainment.IndexOf(getCurrentEt);

            if (getCurrentEt != null)
            {
                _fileHandlerUtil.JsonModel.Entertainment[indexOfEt].EnterName = enterNewName;
                _fileHandlerUtil.JsonModel.Entertainment[indexOfEt].Links = newLink;
                _fileHandlerUtil.JsonModel.Entertainment[indexOfEt].Category = newCategory;
                _fileHandlerUtil.JsonModel.Entertainment[indexOfEt].LastModifiedDate = $"{DateTime.Now:MMMM dd, yyyy}";

                Logs.Info($"[UpdateEntertainment] Update '{enterNewName}' successful.");
                _fileHandlerUtil.SaveFile(EnumFileConstant.ENTERTAINMENTCONSTAT);

                return true;
            }
            Logs.Warn($"[UpdateEntertainment] '{currentEnterName}' doesn't exist.");
            _fileHandlerUtil.SaveFile(EnumFileConstant.ENTERTAINMENTCONSTAT);

            return false;
        }

        public bool DeleteEntertainment(string enterName)
        {
            if (enterName == null) return false;

            var getEt = _fileHandlerUtil.JsonModel.Entertainment.
                Find(e => string.Equals(e.EnterName, enterName, StringComparison.CurrentCultureIgnoreCase));

            if (getEt == null)
            {
                _fileHandlerUtil.SaveFile(EnumFileConstant.ENTERTAINMENTCONSTAT);
                Logs.Warn($"[DeleteEntertainment] '{enterName}' doesn't exist.");

                return false;
            }
            _fileHandlerUtil.JsonModel.Entertainment.Remove(getEt);

            Logs.Info($"[DeleteEntertainment] Deleting '{enterName}' successful.");

            _fileHandlerUtil.SaveFile(EnumFileConstant.ENTERTAINMENTCONSTAT);

            return true;
        }

        public List<Entertainment> GetListEntertainments()
        {
            List<Entertainment> listEt = null;
            try
            {
                listEt = _fileHandlerUtil.JsonModel.Entertainment.ToList();

                _fileHandlerUtil.SaveFile(EnumFileConstant.ENTERTAINMENTCONSTAT);

                return listEt;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public void BackupDatabase(EnumFileConstant enumFile, string backupFolder)
        {
            _fileHandlerUtil.BackUpFileWithFolder(enumFile, backupFolder);
        }

        public void SaveFileTo(string filePath, string tableName)
        {
            _fileHandlerUtil.ExportFile<Entertainment>(filePath.ToLower().EndsWith(".xlsx") ? filePath : filePath.Insert(filePath.Length, ".xlsx"), tableName);
            Logs.Info($"[SaveFileTo] File '{tableName}' was saved at '{filePath}'.");
        }
    }
}
