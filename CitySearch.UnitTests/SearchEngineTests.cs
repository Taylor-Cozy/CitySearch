using Microsoft.VisualStudio.TestTools.UnitTesting;
using CitySearch;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace CitySearch.UnitTests
{
    [TestClass]
    public class SearchEngineTests
    {

        #region Lists and Search Classes
        private List<string> twentyWords;
        private SearchClass twentyWordSearchEngine;

        private List<string> twentyFiveThoudsandWords;
        private SearchClass twentyFiveThouSearchEngine;

        private List<string> oneHundredThousandWords;
        private SearchClass hundredThouSearchEngine;

        private List<string> oneMillionWords;
        private SearchClass oneMilSearchEngine;
        #endregion

        #region Functionality Test Variables
        ICityResult result_correctInput;
        #endregion

        #region Speed Test Variables
        //private Stopwatch stopwatch;
        #endregion

        #region Initialize Tests
        [TestInitialize]
        public void Initialize_AllTests()
        {
            #region 20 Words Setup
            twentyWords = new List<string>();

            twentyWords.Add("Apple");
            twentyWords.Add("Apple");
            twentyWords.Add("Apple");
            twentyWords.Add("Apricot");
            twentyWords.Add("Banana");
            twentyWords.Add("Banana");
            twentyWords.Add("Banana");
            twentyWords.Add("Banana");
            twentyWords.Add("Banana");
            twentyWords.Add("Banana");
            twentyWords.Add("Banana");
            twentyWords.Add("Banana");
            twentyWords.Add("Banana");
            twentyWords.Add("Ban ana");
            twentyWords.Add("Ban ana");
            twentyWords.Add("Ban-ana");
            twentyWords.Add("Ban-ana");
            twentyWords.Add("Bandana");
            twentyWords.Add("Banned");
            twentyWords.Add("Coconut");

            twentyWords.Sort(); // Sorting the " " and "-" properly

            twentyWordSearchEngine = new SearchClass(twentyWords);
            #endregion

            #region 25,000 Words Setup
            
            twentyFiveThoudsandWords = new List<string>();

            string[] twentyFiveKlines = System.IO.File.ReadAllLines(@"..\..\..\Text Files\TwentyFiveThouWords.txt");
            foreach (string x in twentyFiveKlines)
            {
                twentyFiveThoudsandWords.Add(x);
            }

            twentyFiveThouSearchEngine = new SearchClass(twentyFiveThoudsandWords);

            #endregion

            #region 100,000 Words Setup

            oneHundredThousandWords = new List<string>();

            string[] oneHundredThouLines = System.IO.File.ReadAllLines(@"..\..\..\Text Files\OneHundredThouWords.txt");
            foreach (string x in oneHundredThouLines)
            {
                oneHundredThousandWords.Add(x);
            }

            hundredThouSearchEngine = new SearchClass(oneHundredThousandWords);

            #endregion

            #region 1,000,000 Words Setup

            oneMillionWords = new List<string>();

            string[] oneMillionLines = System.IO.File.ReadAllLines(@"..\..\..\Text Files\OneMillionWords.txt");
            foreach (string x in oneMillionLines)
            {
                oneMillionWords.Add(x);
            }

            oneMilSearchEngine = new SearchClass(oneMillionWords);

            #endregion

            #region Initialize Functionality Test
            result_correctInput = twentyWordSearchEngine.Search("Ban");
            #endregion

            #region Initialize Speed Test
            //stopwatch = new Stopwatch();
            #endregion
        }
        #endregion

        #region Functionality Tests
        // Correct Input

        [TestMethod, TestCategory("CorrectInput")]
        public void Test_ReturnsCorrectNumberOfCities()
        {
            // Expected Result: 15 next cities (9 Bananas, 2 Ban Ana, 2 Ban-ana, Bandana, Banned
            Assert.AreEqual(15, result_correctInput.NextCities.Count);

        }

        [TestMethod, TestCategory("CorrectInput")]
        public void Test_ReturnsCorrectCities()
        {
            // Expected Result: 15 next cities (9 Bananas, 2 Ban Ana, 2 Ban-ana, Bandana, Banned
            List<string> correctCities = new List<string> {
                "Ban ana", "Ban ana", "Banana", "Banana", "Banana", 
                "Banana", "Banana", "Banana", "Banana", "Banana", 
                "Banana", "Ban-ana", "Ban-ana", "Bandana", "Banned"};

            CollectionAssert.AreEqual(correctCities, result_correctInput.NextCities.ToList<string>());
        }

        [TestMethod, TestCategory("CorrectInput")]
        public void Test_ReturnsCorrectNumberofNextLetters()
        {

            // Expected Return: 5 Next Letters (a, ' ', -, d, n);
            Assert.AreEqual(5, result_correctInput.NextLetters.Count);
        }

        [TestMethod, TestCategory("CorrectInput")]
        public void Test_ReturnsCorrectNextLetters()
        {
            List<string> correctLetters = new List<string>() { " ", "a", "-", "d", "n"};

            CollectionAssert.AreEqual(correctLetters, result_correctInput.NextLetters.ToList<string>());
        }


        // Lower Edge Input
        [TestMethod, TestCategory("EdgeCase")]
        public void Test_LowerEdge()
        {
            var result = twentyWordSearchEngine.Search("Apple"); // 3 Apples at beginning of list

            Assert.AreEqual(3, result.NextCities.Count);
        }

        // Upper Edge Input
        [TestMethod, TestCategory("EdgeCase")]
        public void Test_UpperEdge()
        {
            var result = twentyWordSearchEngine.Search("Coconut"); // 1 Coconut at beginning of list

            Assert.AreEqual(1, result.NextCities.Count);
        }

        // Incorrect Input (Not in List)
        [TestMethod, TestCategory("IncorrectInput")]
        public void Test_IncorrectInput()
        {
            var result = twentyWordSearchEngine.Search("notinlist");

            Assert.AreEqual(0, result.NextCities.Count);
        }

        #endregion

        #region Speed Tests
        [TestMethod, TestCategory("SpeedTest")]
        public void Test_Speed_TwentyWords()
        {
            // Simulate User Searching for Banned
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            string searchTerm = "Banned";

            for(int i = 1; i <= searchTerm.Length; i++)
            {
               // stopwatch.Start;
                string currentTerm = searchTerm.Substring(0, i);
                stopwatch.Restart();
                var result = twentyWordSearchEngine.Search(currentTerm);
                stopwatch.Stop();

                Console.WriteLine("Time Taken: {0,2}ms {1,8} ticks\t|\tSearch Term: {2,-10}\t|\tNext Cities: {3,5}", stopwatch.ElapsedMilliseconds, stopwatch.ElapsedTicks, currentTerm, result.NextCities.Count);
            }

            Assert.IsTrue(true);
        }

        [TestMethod, TestCategory("SpeedTest")]
        public void Test_Speed_TwentyFiveThousandWords()
        {
            // Simulate User Searching for Banned
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            string searchTerm = "balloon";

            for (int i = 1; i <= searchTerm.Length; i++)
            {
                // stopwatch.Start;
                string currentTerm = searchTerm.Substring(0, i);
                stopwatch.Restart();
                var result = twentyFiveThouSearchEngine.Search(currentTerm);
                stopwatch.Stop();

                Console.WriteLine("Time Taken: {0,2}ms {1,8} ticks\t|\tSearch Term: {2,-10}\t|\tNext Cities: {3,5}", stopwatch.ElapsedMilliseconds, stopwatch.ElapsedTicks, currentTerm, result.NextCities.Count);
            }

            Assert.IsTrue(true);
        }

        [TestMethod, TestCategory("SpeedTest")]
        public void Test_Speed_OneHundredThousandWords()
        {
            // Simulate User Searching for Banned
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            string searchTerm = "progress";

            for (int i = 1; i <= searchTerm.Length; i++)
            {
                // stopwatch.Start;
                string currentTerm = searchTerm.Substring(0, i);
                stopwatch.Restart();
                var result = hundredThouSearchEngine.Search(currentTerm);
                stopwatch.Stop();

                Console.WriteLine("Time Taken: {0,2}ms {1,8} ticks\t|\tSearch Term: {2,-10}\t|\tNext Cities: {3,5}", stopwatch.ElapsedMilliseconds, stopwatch.ElapsedTicks, currentTerm, result.NextCities.Count);
            }

            Assert.IsTrue(true);
        }

        [TestMethod, TestCategory("SpeedTest")]
        public void Test_Speed_OneMillionWords()
        {
            // Simulate User Searching for Banned
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            string searchTerm = "apple";

            for (int i = 1; i <= searchTerm.Length; i++)
            {
                // stopwatch.Start;
                string currentTerm = searchTerm.Substring(0, i);
                stopwatch.Restart();
                var result = oneMilSearchEngine.Search(currentTerm);
                stopwatch.Stop();

                Console.WriteLine("Time Taken: {0,2}ms {1,8} ticks\t|\tSearch Term: {2,-10}\t|\tNext Cities: {3,10}", stopwatch.ElapsedMilliseconds, stopwatch.ElapsedTicks, currentTerm, result.NextCities.Count);
            }

            Assert.IsTrue(true);
        }
        #endregion

    }
}
