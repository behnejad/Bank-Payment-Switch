﻿using AppZoneUI.Framework;
using AppZoneUI.Framework.Mods;
using BankSwitch.Core.Entities;
using BankSwitch.Logic;
using BankSwitch.UI.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankSwitch.UI.SourceNodeManagement
{
   public class ViewSourceNodeList:EntityUI<SourceNodeModel>
    {
       public ViewSourceNodeList()
       {
           WithTitle("View Sink Nodes");
           AddSection()
           .IsFramed()
           .IsCollapsible()
              .WithColumns(
                  new List<Column>()
                    {
                        new Column(new List<IField>()
                        {  
                           Map(x => x.Name).AsSectionField<TextBox>().LabelTextIs("Name"),
                           Map(x => x.Port).AsSectionField<TextBox>().WithLength(30),
                              AddSectionButton()
                            .WithText("Search")
                            .UpdateWith(x=> 
                                {
                                    return x;
                                })
                            }),
                              new Column(
                            new List<IField>()
                            {
                              Map(x => x.HostName).AsSectionField<TextBox>().LabelTextIs("Host Name"),
                               Map(x=>x.IPAddress).AsSectionField<TextBox>().WithLength(30),
                            }),
                    });
               

           AddSection().WithTitle("Source Nodes").IsFramed().IsCollapsible()
           .WithColumns(new List<Column>()
                {
                    new Column(new List<IField>()
                    {
                              HasMany(x =>x.SourceNodes)
                            .AsSectionField<Grid>()
                            .Of<SourceNode>()
                            .WithRowNumbers()
                            .WithColumn(x => x.Name)
                            .WithColumn(x => x.IPAddress)
                            .WithColumn(x => x.HostName)
                            .WithColumn(x => x.Port)
                            .WithRowNumbers()
                            .IsPaged<SourceNodeModel>(10, (x, e) =>
                            { 
                                int total= 0;
                                try
                                {
                                    x.SourceNodes = new SourceNodeManager().GetAllSourceNode(x.Name, x.HostName, x.IPAddress, x.Port, e.Start / e.Limit, e.Limit, out total);
                                    e.TotalCount = total;
                                    System.Web.HttpContext.Current.Session["TotalSinkNode"] = e.TotalCount;
                                    return x;
                                }
                                catch (Exception)
                                {
                                    throw;
                                }
                            }).ApplyMod<ViewDetailsMod>(y => y.Popup<sourceNodeDetail>("Route Details")),
                
                   })
                });
       }
    }
}
