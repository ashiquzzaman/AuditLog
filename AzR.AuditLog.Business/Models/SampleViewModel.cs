using AzR.AuditLog.DataAccess.AuditLog;
using AzR.AuditLog.DataAccess.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AzR.AuditLog.Business.Models
{
    public class SampleModel
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string Lastname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public bool Deleted { get; set; }


        public SampleModel GetData(int id)
        {
            var mod = new SampleModel();
            var ent = new ApplicationDbContext();
            var rec = ent.Samples.FirstOrDefault(s => s.Id == id);
            if (rec == null) return mod;
            mod.Id = rec.Id;
            mod.FirstName = rec.FirstName;
            mod.Lastname = rec.LastName;
            mod.DateOfBirth = rec.DateOfBirth;
            return mod;
        }

        public void DeleteRecord(int id)
        {
            var ent = new ApplicationDbContext();
            Sample rec = ent.Samples.FirstOrDefault(s => s.Id == id);
            if (rec == null) return;
            var dummyObject = new Sample(); // Storage of this null object shows data after delete = nix, naught, nothing!
            rec.Deleted = true;
            ent.SaveChanges();
            ChangeLog.Create<Sample>(ActionType.Delete, id, rec, dummyObject);
        }


        public List<SampleModel> GetAllData(bool showDeleted)
        {
            var ent = new ApplicationDbContext();

            var searchResults = showDeleted
                ? ent.Samples.ToList()
                : ent.Samples.Where(s => s.Deleted == false).ToList();

            return searchResults.Select(record => new SampleModel
            {
                Id = record.Id,
                FirstName = record.FirstName,
                Lastname = record.LastName,
                DateOfBirth = record.DateOfBirth,
                Deleted = record.Deleted
            })
                .ToList();
        }

        public bool UpdateRecord(SampleModel rec)
        {
            bool rslt = false;
            var ent = new ApplicationDbContext();
            var dbRec = ent.Samples.FirstOrDefault(s => s.Id == rec.Id);
            if (dbRec == null) return false;
            // audit process 1 - gather old values
            var oldRecord = new SampleModel
            {
                Id = dbRec.Id,
                FirstName = dbRec.FirstName,
                Lastname = dbRec.LastName,
                DateOfBirth = dbRec.DateOfBirth
            };
            // update the live record
            dbRec.FirstName = rec.FirstName;
            dbRec.LastName = rec.Lastname;
            dbRec.DateOfBirth = rec.DateOfBirth;
            ent.SaveChanges();

            //ChangeLog.Create<Sample>(ActionType.Update, Rec.Id, oldRecord, Rec);

            return true;
        }

        public void CreateRecord(SampleModel rec)
        {

            var ent = new ApplicationDbContext();
            var dbRec = new Sample
            {
                FirstName = rec.FirstName,
                LastName = rec.Lastname,
                DateOfBirth = rec.DateOfBirth
            };
            ent.Samples.Add(dbRec);
            ent.SaveChanges(); // save first so we get back the dbRec.Id for audit tracking
            var dummyObject = new Sample(); // Storage of this null object shows data before creation = nix, naught, nothing!

            //CreateAuditTrail(ActionType.Create, dbRec.Id, dummyObject, dbRec);

        }


        public List<ChangeLog> GetAudit(int id)
        {
            var rslt = new List<ChangeLog>();
            var ent = new ApplicationDbContext();
            var auditTrail = ent.AuditLogs.Where(s => s.KeyFieldId == id).OrderByDescending(s => s.ActionTime); // we are looking for audit-history of the record selected.
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
                rslt.Add(change);
            }
            return rslt;
        }


    }


}