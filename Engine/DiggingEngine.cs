using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.Diagnostics;
using BlakeSharp;
using System.Threading.Tasks;
using System.Linq;
using System.ComponentModel;

namespace Engine
{

    public enum Algorithm
    {
        sha256,
        blake,
        sha256d
    }


    public class TestParametr
    {
        public TestParametr()
        {
            algorithm = Algorithm.sha256;
            numberOfZeroInBegin = 3;
            stepToFInd = 10;
            increasNoZTo = 3;
            valueToChangeTimeInSearch = 900000000;
            numberOfRepeating = 15;
        }
        public Algorithm algorithm;
        public int numberOfZeroInBegin;
        public int stepToFInd;
        public int increasNoZTo;
        public int valueToChangeTimeInSearch;
        public int numberOfRepeating;
    }

    public class Block
    {
        public Block()
        {
            previousHash = "0000000000000000000000000000000000000000000000000000000000000000";
            text = "first text";
            time = 3000000;
            isCorecthash = false;
            randomValue = 0;
        }

        public string previousHash;
        public string text;
        public int time;
        public string hash;
        public bool isCorecthash;
        public int randomValue;
        public bool CheckNumberOfZero(int numberOfZero)
        {
            var substring = hash.Substring(0, numberOfZero);
            return substring.All(c => "0".Contains(c));
        }

        public bool CheckNumberOfZero(int numberOfZero, string hashToTest)
        {
            var substring = hashToTest.Substring(0, numberOfZero);
            return substring.All(c => "0".Contains(c));
        }

    }

    public class DiggingEngine
    {
        Blake256 blake256 = new Blake256();
        SHA256 sha256 = SHA256.Create();

        public byte[] GetHash(Algorithm algorithm, string textToHash)
        {
            var bytesOftext = System.Text.Encoding.Default.GetBytes(textToHash);
            switch (algorithm)
            {
                case Algorithm.sha256:
                    return sha256.ComputeHash(bytesOftext);
                case Algorithm.sha256d:
                    return sha256.ComputeHash(sha256.ComputeHash(bytesOftext));
                case Algorithm.blake:
                    return blake256.ComputeHash(bytesOftext);
            }
            return new byte[0];
        }

        public string GetHashString(Algorithm algorithm, Block blockToHash)
        {
            var hashbyte = GetHash(algorithm, blockToHash.text + blockToHash.time.ToString() + blockToHash.randomValue.ToString());
            return System.Text.Encoding.UTF8.GetString(hashbyte);
        }

        public long DiggingTestParallel(TestParametr parametrs)
        {
            return DiggingTestParallel(parametrs.algorithm, parametrs.numberOfZeroInBegin, parametrs.stepToFInd, parametrs.increasNoZTo, parametrs.valueToChangeTimeInSearch);
        }

        public long DiggingTestParallel(Algorithm algorithm, int numberOfZeroInBegin = 1, int stepToFInd = 2, int increasNoZTo = 2, int valueToChangeTimeInSearch = 900000000)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();

            // Parallel.For(numberOfZeroInBegin, increasNoZTo + 1, NoZ =>
            for (int NoZ = numberOfZeroInBegin; NoZ <= increasNoZTo; NoZ++)
            {
                Block block = new Block();
                for (int i = 0; i < stepToFInd; i++)
                {
                    // Console.WriteLine("step");
                    block.isCorecthash = false;
                    block.hash = "";
                    while (block.isCorecthash != true)
                    {
                        Random rnd = new Random(DateTime.Now.Minute);

                        Parallel.For(0, valueToChangeTimeInSearch, (j, state) =>
                        {
                            block.randomValue = rnd.Next();
                            var hash = GetHashString(algorithm, block);
                            if (block.CheckNumberOfZero(NoZ, hash) && block.isCorecthash != true)
                            {
                                block.isCorecthash = true;
                                //     Console.WriteLine(hash+"\n\n\n");
                                state.Break();
                            }
                        });
                              //     Console.WriteLine("za state.break;");
                        block.time++;
                    }
                }
                    // Console.WriteLine("koniec iteracji paralell for");
            }//);
             //   Console.WriteLine("za głównym paralell;");
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }


        public long DiggingTest(TestParametr parametrs)
        {
            return DiggingTest(parametrs.algorithm, parametrs.numberOfZeroInBegin, parametrs.stepToFInd, parametrs.increasNoZTo, parametrs.valueToChangeTimeInSearch);
        }

        public long DiggingTest(Algorithm algorithm, int numberOfZeroInBegin = 3, int stepToFInd = 2, int increasNoZTo = 3, int valueToChangeTimeInSearch = 100000000)
        {
            Stopwatch stopwatch = Stopwatch.StartNew();
            for (int NoZ = numberOfZeroInBegin; NoZ <= increasNoZTo; NoZ++)
            {
                Block block = new Block();
                for (int i = 0; i < stepToFInd; i++)
                {
                    block.isCorecthash = false;
                    block.hash = "";
                    while (block.isCorecthash != true)
                    {
                        Random rnd = new Random(DateTime.Now.Minute);

                        //tego fora jako parallel
                        for (int j = 0; j < valueToChangeTimeInSearch; j++)
                        {
                            block.randomValue = rnd.Next();
                            block.hash = GetHashString(algorithm, block);
                            if (block.CheckNumberOfZero(NoZ))
                            {
                                block.isCorecthash = true;
                                //   Console.WriteLine(block.hash + "\n\n\n");
                                break;
                            }
                        }
                        block.time++;
                    }
                }
            }
            stopwatch.Stop();
            return stopwatch.ElapsedMilliseconds;
        }

    }
}
