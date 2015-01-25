﻿using MindMate.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MindMate.Plugins.Tasks
{
    public class TaskPlugin : IPlugin
    {
        public const string ATT_DUE_DATE = "Due Date";

        private DateTimePicker dateTimePicker; 

        public void Initialize(PluginManager pluginMgr)
        {
            dateTimePicker = new DateTimePicker();
        }

        public MenuItem[] CreateContextMenuItemsForNode()
        {
            var t2 = new MenuItem("Quick Due Dates");
 
            t2.AddDropDownItem(new MenuItem("Today"));
            t2.AddDropDownItem(new MenuItem("Tomorrow"));
            t2.AddDropDownItem(new MenuItem("This Week"));
            t2.AddDropDownItem(new MenuItem("Next Week"));
            t2.AddDropDownItem(new MenuItem("This Month"));
            t2.AddDropDownItem(new MenuItem("Next Month"));
            t2.AddDropDownItem(new MenuItem("No Date"));

            var t3 = new MenuItem("Complete Task");
            t3.Opening = Complete_Opening;

            MenuItem[] menuItems = new MenuItem[] 
            {
                new MenuItem("Set Due Date ...", null, SetDueDate_Click),
                t2,
                t3
            };

            return menuItems;
        }

        private void Complete_Opening(MenuItem menuItem, SelectedNodes nodes)
        {
            for (int i = 0; i < nodes.Count; i++ )
            {
                if (!nodes[i].GetAttribute(ATT_DUE_DATE).IsEmpty())
                {
                    menuItem.Enabled = true;
                    return;
                }
            }
            menuItem.Enabled = false;
        }

        private void SetDueDate_Click(MenuItem menu, SelectedNodes nodes)
        {
            dateTimePicker.Value = DateTime.Today.Date;
            if(dateTimePicker.ShowDialog() == DialogResult.OK)
            {
                for (int i = 0; i < nodes.Count; i++)
                {
                    MapNode node = nodes[i];

                    MapTree.AttributeSpec aspec = node.Tree.GetAttributeSpec(ATT_DUE_DATE);
                    if (aspec == null)
                        aspec = CreateDueDateAttributeSpec(node.Tree);

                    MapNode.Attribute att = node.GetAttribute(aspec);
                    if (att.IsEmpty())
                        att.AttributeSpec = aspec;

                    att.value = dateTimePicker.Value.ToShortDateString();
                    node.AddUpdateAttribute(att);

                }
            }
        }

        private MapTree.AttributeSpec CreateDueDateAttributeSpec(MapTree tree)
        {
            return new MapTree.AttributeSpec(
                tree, ATT_DUE_DATE, true, MapTree.AttributeDataType.DateTime, 
                MapTree.AttributeListOption.NoList, null, MapTree.AttributeType.System);
        }
                        
        public void CreateMainMenuItems(out MenuItem[] menuItems, out MainMenuLocation position)
        {
            throw new NotImplementedException();
        }

        public void RegisterTreeEvents(Model.MapTree tree)
        {
            throw new NotImplementedException();
        }

        public void UnregisterTreeEvents(Model.MapTree tree)
        {
            throw new NotImplementedException();
        }
                       
        public void CreateMainMenuItems(out ToolStripMenuItem[] menuItems, out MainMenuLocation location)
        {
            throw new NotImplementedException();
        }
        
        
    }
}
