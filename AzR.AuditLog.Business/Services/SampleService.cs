﻿using AzR.AuditLog.Business.Models;
using AzR.AuditLog.DataAccess.AuditLog;
using AzR.AuditLog.DataAccess.Entities;
using AzR.AuditLog.DataAccess.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzR.AuditLog.Business.Services
{
    public class SampleService : ISampleService
    {
        private ISampleRepository _sample;
        private IAuditLogRepository _auditLog;
        public SampleService(ISampleRepository sample, IAuditLogRepository auditLog)
        {
            _sample = sample;
            _auditLog = auditLog;
        }
        public SampleViewModel Get(int id)
        {
            var model = new SampleViewModel();
            var sample = _sample.Find(s => s.Id == id);
            if (sample == null) return model;
            model.Id = sample.Id;
            model.FirstName = sample.FirstName;
            model.LastName = sample.LastName;
            model.DateOfBirth = sample.DateOfBirth;
            model.Active = sample.Active;
            return model;
        }

        public void Delete(int id)
        {
            var model = _sample.First(s => s.Id == id);
            model.Active = false;
            _sample.SaveChanges();
        }


        public List<SampleViewModel> GetAll(bool showAll)
        {

            var results = showAll
                ? _sample.GetAll.ToList()
                : _sample.FindAll(s => s.Active).ToList();

            return results.Select(record => new SampleViewModel
            {
                Id = record.Id,
                FirstName = record.FirstName,
                LastName = record.LastName,
                DateOfBirth = record.DateOfBirth,
                Active = record.Active
            }).ToList();

        }

        public bool Update(SampleViewModel model)
        {
            var sample = new Sample
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Active = model.Active
            };
            _sample.Update(sample);
            return true;
        }

        public void Create(SampleViewModel model)
        {

            var sample = new Sample
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                DateOfBirth = model.DateOfBirth,
                Active = model.Active
            };
            _sample.Create(sample);
        }


        public List<ChangeLog> GetAudit(int id)
        {
            var logs = new List<ChangeLog>();
            var auditTrail = _auditLog.FindAll(s => s.KeyFieldId == id.ToString()).OrderByDescending(s => s.ActionTime); // we are looking for audit-history of the record selected.
            foreach (var record in auditTrail)
            {
                var change = new ChangeLog
                {
                    ActionBy = record.ActionBy,
                    ActionTime = record.ActionTime.ToString(),
                    ActionType = record.ActionType,
                    ActionTypeName = Enum.GetName(typeof(ActionType), record.ActionType)
                };
                var delta = JsonConvert.DeserializeObject<List<ObjectChangeLog>>(record.ValueChange);
                change.Changes.AddRange(delta);
                logs.Add(change);
            }
            return logs;
        }


    }
}
