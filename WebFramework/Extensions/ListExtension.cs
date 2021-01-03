using System.Collections.Generic;
using System.Linq;
using Model.Bases;

namespace WebFramework.Extensions
{
    public static class ListExtension
    {
        public static List<T> ConvertToNode<T>(this List<T> list) where T : RecEntity<T>
        {
            //STP I
            var groups = (
                           from    info in list
                           orderby info.ParentId
                           group   info by info.ParentId into group_info
                           select  group_info
                         );
            
            /*
             * Result groups
             *
             * IEnumarable<Dictionary<int?, T>> groups = new List<Dictionary<int?, T>>  |  int? is ParentId
             * {
             *     new Dictionary<int?, T>()
             *     {
             *         {null, T1},
             *         {null, T2}
             *     }
             *     ,
             *     new Dictionary<int?, T>()
             *     {
             *         {3, T3},
             *         {3, T4}
             *     }
             *     ,
             * 
             *     .
             *     .
             *     .
             * }
             */

            //STP II
            var groups_filter = groups.Where(item => !item.Key.HasValue);
            var roots_list    = groups_filter.FirstOrDefault().ToList();
            
            //STP III
            if (roots_list.Count > 0)
            {
                var dict = groups.Where(item => item.Key.HasValue).ToDictionary(item => item.Key.Value, item => item.ToList());
                for (int i = 0; i < roots_list.Count; i++)
                {
                    AddChildren<T>(roots_list[i], dict);
                }
            }

            //STP IV
            return roots_list;
        }
        
        private static void AddChildren<T>(T node, Dictionary<int, List<T>> data) where T : RecEntity<T>
        {
            if (data.ContainsKey(node.Id))
            {
                node.Node = data[node.Id];
                for (int i = 0; i < node.Node.Count; i++)
                {
                    AddChildren<T>(node.Node[i], data);
                }
            }
            else
            {
                node.Node = new List<T>();
            }
        }
    }
}