// Skeleton implementation written by Joe Zachary for CS 3500, September 2013.
// Version 1.1 (Fixed error in comment for RemoveDependency.)
// Version 1.2 - Daniel Kopta 
//               (Clarified meaning of dependent and dependee.)
//               (Clarified names in solution/project structure.)


/// <summary>
/// Author: Nik McKnight
/// Date: 1/26/22
/// Course: CS3500, University of Utah, School of Computing
/// Copyright: CS3500 and Nik McKnight - this work may not be copied for use in Academic Coursework.
/// 
/// I, Nik McKnight, certify that I wrote this code, not counting the starter code, from scratch and did not copy it in part or whole from  
/// another source.  All references used in the completion of the assignment are cited in my README file. 
/// 
/// File Contents
/// 
/// </summary>

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpreadsheetUtilities
{

    /// <summary>
    /// (s1,t1) is an ordered pair of strings
    /// t1 depends on s1; s1 must be evaluated before t1
    /// 
    /// A DependencyGraph can be modeled as a set of ordered pairs of strings.  Two ordered pairs
    /// (s1,t1) and (s2,t2) are considered equal if and only if s1 equals s2 and t1 equals t2.
    /// Recall that sets never contain duplicates.  If an attempt is made to add an element to a 
    /// set, and the element is already in the set, the set remains unchanged.
    /// 
    /// Given a DependencyGraph orderedPairs:
    /// 
    ///    (1) If s is a string, the set of all strings t such that (s,t) is in orderedPairs is called dependents(s).
    ///        (The set of things that depend on s)    
    ///        
    ///    (2) If s is a string, the set of all strings t such that (t,s) is in orderedPairs is called dependees(s).
    ///        (The set of things that s depends on) 
    //
    // For example, suppose orderedPairs = {("a", "b"), ("a", "c"), ("b", "d"), ("d", "d")}
    //     dependents("a") = {"b", "c"}
    //     dependents("b") = {"d"}
    //     dependents("c") = {}
    //     dependents("d") = {"d"}
    //     dependees("a") = {}
    //     dependees("b") = {"a"}
    //     dependees("c") = {"a"}
    //     dependees("d") = {"b", "d"}
    /// </summary>
    public class DependencyGraph
    {
        // Graph that contains all ordered pairs in the format (dependee, dependent)
        readonly HashSet<Tuple<string, string>> orderedPairs;

        // Hashtables for tracking dependents and dependees of a given cell
        readonly Hashtable dependentsTable;
        readonly Hashtable dependeesTable;

        /// <summary>
        /// Creates an empty DependencyGraph.
        /// </summary>
        public DependencyGraph()
        {
            orderedPairs = new HashSet<Tuple<string, string>>();
            dependentsTable = new Hashtable();
            dependeesTable = new Hashtable();
        }


        /// <summary>
        /// The number of ordered pairs in the DependencyGraph.
        /// </summary>
        public int Size
        {
            get {
                return orderedPairs.Count;
            }
        }


        /// <summary>
        /// The size of dependees(s).
        /// This property is an example of an indexer.  If orderedPairs is a DependencyGraph, you would
        /// invoke it like this:
        /// orderedPairs["a"]
        /// It should return the size of dependees("a")
        /// </summary>
        public int this[string dependent]
        {
            get {
                HashSet<string> dependees = (HashSet<string>)dependeesTable[dependent];
                try
                {
                    return dependees.Count;
                }
                catch (Exception ex)
                {
                    return 0;
                }
            }
        }


        /// <summary>
        /// Reports whether dependents(s) is non-empty.
        /// </summary>
        public bool HasDependents(string s)
        {
            return dependentsTable.ContainsKey(s);
        }


        /// <summary>
        /// Reports whether dependees(s) is non-empty.
        /// </summary>
        public bool HasDependees(string s)
        {
            return dependeesTable.ContainsKey(s);
        }


        /// <summary>
        /// Enumerates dependents(s).
        /// </summary>
        public IEnumerable<string> GetDependents(string dependee)
        {
            HashSet<string> dependents = new HashSet<string>();
            if (dependentsTable.ContainsKey(dependee))
            {
                dependents = (HashSet<string>)dependentsTable[dependee];
            }
            return dependents;
        }

        /// <summary>
        /// Enumerates dependees(s).
        /// </summary>
        public IEnumerable<string> GetDependees(string dependent)
        {
            HashSet<string> dependees = new HashSet<string>();
            if (dependeesTable.ContainsKey(dependent))
            {
                dependees = (HashSet<string>)dependeesTable[dependent];
            }
            return dependees;
        }


        /// <summary>
        /// <para>Adds the ordered pair (s,t), if it doesn't exist</para>
        /// 
        /// <para>This should be thought of as:</para>   
        /// 
        ///   t depends on s
        ///
        /// </summary>
        /// <param name="s"> s must be evaluated first. T depends on S</param>
        /// <param name="t"> t cannot be evaluated until s is</param>        /// 
        public void AddDependency(string s, string t)
        {
            // add given ordered pair to the set of ordered pairs
            orderedPairs.Add(new Tuple<string, string>(s, t));

            // add relationship to both hashtables
            AddToTable(s, t, dependentsTable);
            AddToTable(t, s, dependeesTable);
        }


        /// <summary>
        /// Removes the ordered pair (s,t), if it exists and updates hashtables
        /// </summary>
        /// <param name="s"></param>
        /// <param name="t"></param>
        public void RemoveDependency(string s, string t)
        {
            orderedPairs.Remove(new Tuple<string, string>(s, t));
            RemoveFromTable(s, t, dependentsTable);
            RemoveFromTable(t, s, dependeesTable);
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (s,r).  Then, for each
        /// t in newDependents, adds the ordered pair (s,t) and updates hashtables.
        /// </summary>
        public void ReplaceDependents(string s, IEnumerable<string> newDependents)
        {
            //Remove dependencies
            var oldDependents = new HashSet<string>();
            oldDependents = (HashSet<string>)dependentsTable[s];
            if (oldDependents != null)
            {
                foreach (string t in oldDependents)
                {
                    RemoveDependency(s, t);
                }
            }

            //Add dependencies
            foreach (var tempDependent in newDependents)
            {
                AddDependency(s, tempDependent);
            }
        }


        /// <summary>
        /// Removes all existing ordered pairs of the form (r,s).  Then, for each 
        /// t in newDependees, adds the ordered pair (t,s) and updates hashtables.
        /// </summary>
        public void ReplaceDependees(string s, IEnumerable<string> newDependees)
        {
            //Remove dependencies
            var oldDependees = new HashSet<string>();
            oldDependees = (HashSet<string>)dependeesTable[s];
            if (oldDependees != null)
            {
                foreach (string t in oldDependees)
                {
                    RemoveDependency(t, s);
                }
            }

            //Add dependencies
            foreach (var tempDependee in newDependees)
            {
                AddDependency(tempDependee, s);
            }
        }

        /// <summary>
        /// Adds a single value to the given key's list of dependents/dependees in the given Hashtable
        /// Adds the key if it does not exist and adds to the existing key if it does.
        /// </summary>
        /// <param name="key"> The key whose dependents or dependees we want to add to</param>
        /// <param name="value"> the dependent or dependee to add</param>
        /// <param name="table"> the given hashTable</param>
        private void AddToTable(string key, string value, Hashtable table)
        {
            HashSet<string> tempSet;
            if (table.ContainsKey(key))
            {
                tempSet = (HashSet<string>)table[key];
                tempSet.Add(value);
                table[key] = tempSet;
            }
            else
            {
                tempSet = new HashSet<string>();
                tempSet.Add(value);
                table.Add(key, tempSet);
            }
            
        }

        /// <summary>
        /// Removes a single value from the given key's list of dependents/dependees (if it exists) in the given Hashtable
        /// Removes the key if it no longer has any values.
        /// </summary>
        /// <param name="key"> The key whose dependents or dependees we want to add to</param>
        /// <param name="value"> the dependent or dependee to add</param>
        /// <param name="table"> the given hashTable</param>

        private void RemoveFromTable(string key, string value, Hashtable table)
        {
            HashSet<string> tempSet;
            if (table.ContainsKey(key))
            {
                tempSet = (HashSet<string>)table[key];
                tempSet.Remove(value);
                if (tempSet.Count == 0)
                {
                    table.Remove(key);
                }
                else
                {
                    table[key] = tempSet;
                }
            }
        }

    }

}
