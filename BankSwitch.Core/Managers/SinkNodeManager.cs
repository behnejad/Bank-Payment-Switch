﻿using BankSwitch.Core.DAO;
using BankSwitch.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.Core.Managers
{
   public class SinkNodeManager
    {
       private SinkNodeDAO _db;
       public SinkNodeManager(SinkNodeDAO db)
       {
           _db = db;
       }
       public SinkNodeManager()
       {
           _db = new SinkNodeDAO();
       } 
      public bool AddSinkNode(SinkNode sinkNode)
       {
           bool result = false;
           try
           {
               var node = _db.GetAll<SinkNode>().FirstOrDefault(x => x.IPAddress == sinkNode.IPAddress);
               if (node != null)
               {
                   throw new Exception("This Sink Node With this IP already Exist");
               }
               else
               {
                result= _db.Add(sinkNode);
               }
               return result;
           }
           catch (Exception ex)
           {
               _db.Rollback();
               throw ex;
           }
       }
     public bool Edit(SinkNode model)
      {
          try
          {
            bool result = false;
             var sinkNode = _db.GetAll<SinkNode>().FirstOrDefault(x => x.IPAddress == model.IPAddress);
              if (sinkNode != null)
              {
                  sinkNode.Name = model.Name;
                  sinkNode.HostName = model.HostName;
                  sinkNode.IPAddress = model.IPAddress;
                  sinkNode.Port = model.Port;
                  sinkNode.IsActive = model.IsActive;
                result= _db.Update(sinkNode);
              }
              return result;
          }
          catch (Exception ex)
          {
             _db.Rollback();
             throw ex;
          }
      }
    }
}