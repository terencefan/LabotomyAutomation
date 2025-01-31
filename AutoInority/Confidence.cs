﻿using System;

using UnityEngine;

namespace AutoInority
{
    public class Confidence
    {
        private static readonly long[][] _table = new long[][] {
            new long[]{1},
            new long[]{1,1},
            new long[]{1,2,1},
            new long[]{1,3,3,1},
            new long[]{1,4,6,4,1},
            new long[]{1,5,10,10,5,1},
            new long[]{1,6,15,20,15,6,1},
            new long[]{1,7,21,35,35,21,7,1},
            new long[]{1,8,28,56,70,56,28,8,1},
            new long[]{1,9,36,84,126,126,84,36,9,1},
            new long[]{1,10,45,120,210,252,210,120,45,10,1},
            new long[]{1,11,55,165,330,462,462,330,165,55,11,1},
            new long[]{1,12,66,220,495,792,924,792,495,220,66,12,1},
            new long[]{1,13,78,286,715,1287,1716,1716,1287,715,286,78,13,1},
            new long[]{1,14,91,364,1001,2002,3003,3432,3003,2002,1001,364,91,14,1},
            new long[]{1,15,105,455,1365,3003,5005,6435,6435,5005,3003,1365,455,105,15,1},
            new long[]{1,16,120,560,1820,4368,8008,11440,12870,11440,8008,4368,1820,560,120,16,1},
            new long[]{1,17,136,680,2380,6188,12376,19448,24310,24310,19448,12376,6188,2380,680,136,17,1},
            new long[]{1,18,153,816,3060,8568,18564,31824,43758,48620,43758,31824,18564,8568,3060,816,153,18,1},
            new long[]{1,19,171,969,3876,11628,27132,50388,75582,92378,92378,75582,50388,27132,11628,3876,969,171,19,1},
            new long[]{1,20,190,1140,4845,15504,38760,77520,125970,167960,184756,167960,125970,77520,38760,15504,4845,1140,190,20,1},
            new long[]{1,21,210,1330,5985,20349,54264,116280,203490,293930,352716,352716,293930,203490,116280,54264,20349,5985,1330,210,21,1},
            new long[]{1,22,231,1540,7315,26334,74613,170544,319770,497420,646646,705432,646646,497420,319770,170544,74613,26334,7315,1540,231,22,1},
            new long[]{1,23,253,1771,8855,33649,100947,245157,490314,817190,1144066,1352078,1352078,1144066,817190,490314,245157,100947,33649,8855,1771,253,23,1},
            new long[]{1,24,276,2024,10626,42504,134596,346104,735471,1307504,1961256,2496144,2704156,2496144,1961256,1307504,735471,346104,134596,42504,10626,2024,276,24,1},
            new long[]{1,25,300,2300,12650,53130,177100,480700,1081575,2042975,3268760,4457400,5200300,5200300,4457400,3268760,2042975,1081575,480700,177100,53130,12650,2300,300,25,1},
            new long[]{1,26,325,2600,14950,65780,230230,657800,1562275,3124550,5311735,7726160,9657700,10400600,9657700,7726160,5311735,3124550,1562275,657800,230230,65780,14950,2600,325,26,1},
            new long[]{1,27,351,2925,17550,80730,296010,888030,2220075,4686825,8436285,13037895,17383860,20058300,20058300,17383860,13037895,8436285,4686825,2220075,888030,296010,80730,17550,2925,351,27,1},
            new long[]{1,28,378,3276,20475,98280,376740,1184040,3108105,6906900,13123110,21474180,30421755,37442160,40116600,37442160,30421755,21474180,13123110,6906900,3108105,1184040,376740,98280,20475,3276,378,28,1},
            new long[]{1,29,406,3654,23751,118755,475020,1560780,4292145,10015005,20030010,34597290,51895935,67863915,77558760,77558760,67863915,51895935,34597290,20030010,10015005,4292145,1560780,475020,118755,23751,3654,406,29,1},
            new long[]{1,30,435,4060,27405,142506,593775,2035800,5852925,14307150,30045015,54627300,86493225,119759850,145422675,155117520,145422675,119759850,86493225,54627300,30045015,14307150,5852925,2035800,593775,142506,27405,4060,435,30,1},
            new long[]{1,31,465,4495,31465,169911,736281,2629575,7888725,20160075,44352165,84672315,141120525,206253075,265182525,300540195,300540195,265182525,206253075,141120525,84672315,44352165,20160075,7888725,2629575,736281,169911,31465,4495,465,31,1},
            new long[]{1,32,496,4960,35960,201376,906192,3365856,10518300,28048800,64512240,129024480,225792840,347373600,471435600,565722720,601080390,565722720,471435600,347373600,225792840,129024480,64512240,28048800,10518300,3365856,906192,201376,35960,4960,496,32,1},
            new long[]{1,33,528,5456,40920,237336,1107568,4272048,13884156,38567100,92561040,193536720,354817320,573166440,818809200,1037158320,1166803110,1166803110,1037158320,818809200,573166440,354817320,193536720,92561040,38567100,13884156,4272048,1107568,237336,40920,5456,528,33,1},
            new long[]{1, 34, 561, 5984, 46376, 278256, 1344904, 5379616, 18156204, 52451256, 131128140, 286097760, 548354040, 927983760, 1391975640, 1855967520, 2203961430, 2333606220, 2203961430, 1855967520, 1391975640, 927983760, 548354040, 286097760, 131128140, 52451256, 18156204, 5379616, 1344904, 278256, 46376, 5984, 561, 34, 1},
            new long[]{1, 35, 595, 6545, 52360, 324632, 1623160, 6724520, 23535820, 70607460, 183579396, 417225900, 834451800, 1476337800, 2319959400, 3247943160, 4059928950, 4537567650, 4537567650, 4059928950, 3247943160, 2319959400, 1476337800, 834451800, 417225900, 183579396, 70607460, 23535820, 6724520, 1623160, 324632, 52360, 6545, 595, 35, 1},
        };

        private static double[] _distribution = new double[]
        {
            0.0, 0.004, 0.008, 0.012, 0.016, 0.0199, 0.0239, 0.0279, 0.0319, 0.0359,
            0.0398, 0.0438, 0.0478, 0.0517, 0.0557, 0.0596, 0.0636, 0.0675, 0.0714, 0.0753,
            0.0793, 0.0832, 0.0871, 0.091, 0.0948, 0.0987, 0.1026, 0.1064, 0.1103, 0.1141,
            0.1179, 0.1217, 0.1255, 0.1293, 0.1331, 0.1368, 0.1406, 0.1443, 0.148, 0.1517,
            0.1554, 0.1591, 0.1628, 0.1664, 0.17, 0.1736, 0.1772, 0.1808, 0.1844, 0.1879,
            0.1915, 0.195, 0.1985, 0.2019, 0.2054, 0.2088, 0.2123, 0.2157, 0.219, 0.2224,
            0.2257, 0.2291, 0.2324, 0.2357, 0.2389, 0.2422, 0.2454, 0.2486, 0.2517, 0.2549,
            0.258, 0.2611, 0.2642, 0.2673, 0.2704, 0.2734, 0.2764, 0.2794, 0.2823, 0.2852,
            0.2881, 0.291, 0.2939, 0.2967, 0.2995, 0.3023, 0.3051, 0.3078, 0.3106, 0.3133,
            0.3159, 0.3186, 0.3212, 0.3238, 0.3264, 0.3289, 0.3315, 0.334, 0.3365, 0.3389,
            0.3413, 0.3438, 0.3461, 0.3485, 0.3508, 0.3531, 0.3554, 0.3577, 0.3599, 0.3621,
            0.3643, 0.3665, 0.3686, 0.3708, 0.3729, 0.3749, 0.377, 0.379, 0.381, 0.383,
            0.3849, 0.3869, 0.3888, 0.3907, 0.3925, 0.3944, 0.3962, 0.398, 0.3997, 0.4015,
            0.4032, 0.4049, 0.4066, 0.4082, 0.4099, 0.4115, 0.4131, 0.4147, 0.4162, 0.4177,
            0.4192, 0.4207, 0.4222, 0.4236, 0.4251, 0.4265, 0.4279, 0.4292, 0.4306, 0.4319,
            0.4332, 0.4345, 0.4357, 0.437, 0.4382, 0.4394, 0.4406, 0.4418, 0.4429, 0.4441,
            0.4452, 0.4463, 0.4474, 0.4484, 0.4495, 0.4505, 0.4515, 0.4525, 0.4535, 0.4545,
            0.4554, 0.4564, 0.4573, 0.4582, 0.4591, 0.4599, 0.4608, 0.4616, 0.4625, 0.4633,
            0.4641, 0.4649, 0.4656, 0.4664, 0.4671, 0.4678, 0.4686, 0.4693, 0.4699, 0.4706,
            0.4713, 0.4719, 0.4726, 0.4732, 0.4738, 0.4744, 0.475, 0.4756, 0.4761, 0.4767,
            0.4772, 0.4778, 0.4783, 0.4788, 0.4793, 0.4798, 0.4803, 0.4808, 0.4812, 0.4817,
            0.4821, 0.4826, 0.483, 0.4834, 0.4838, 0.4842, 0.4846, 0.485, 0.4854, 0.4857,
            0.4861, 0.4864, 0.4868, 0.4871, 0.4875, 0.4878, 0.4881, 0.4884, 0.4887, 0.489,
            0.4893, 0.4896, 0.4898, 0.4901, 0.4904, 0.4906, 0.4909, 0.4911, 0.4913, 0.4916,
            0.4918, 0.492, 0.4922, 0.4925, 0.4927, 0.4929, 0.4931, 0.4932, 0.4934, 0.4936,
            0.4938, 0.494, 0.4941, 0.4943, 0.4945, 0.4946, 0.4948, 0.4949, 0.4951, 0.4952,
            0.4953, 0.4955, 0.4956, 0.4957, 0.4959, 0.496, 0.4961, 0.4962, 0.4963, 0.4964,
            0.4965, 0.4966, 0.4967, 0.4968, 0.4969, 0.497, 0.4971, 0.4972, 0.4973, 0.4974,
            0.4974, 0.4975, 0.4976, 0.4977, 0.4977, 0.4978, 0.4979, 0.4979, 0.498, 0.4981,
            0.4981, 0.4982, 0.4982, 0.4983, 0.4984, 0.4984, 0.4985, 0.4985, 0.4986, 0.4986,
            0.4987, 0.4987, 0.4987, 0.4988, 0.4988, 0.4989, 0.4989, 0.4989, 0.499, 0.499
        };

        public static float InRange(int count, float prob, int from = 0, int to = -1)
        {
            to = to < 0 ? count : Math.Min(count, to);
            var r = 0f;
            for (int i = from; i <= to; i++)
            {
                r += P(count, i, prob);
            }
            Log.Debug($"{count}, {prob}, {from}, {to}: {r}");
            return r;
        }

        public static long NCR(int n, int r)
        {
            if (n > _table.Length || r > n)
            {
                Log.Error(new Exception("nCr out of bound.")); return -1;
            }
            return _table[n][r];
        }

        public static float P(int n, int r, float p)
        {
            return Mathf.Pow(p, r) * Mathf.Pow(1f - p, n - r) * NCR(n, r);
        }

        public static float Survive(float maxPoints, float damage, float prob, int count)
        {
            maxPoints -= 1;
            if (maxPoints / damage >= count)
            {
                return 1f;
            }

            var r = 0f;
            for (int i = 0; i <= count; i++)
            {
                if (i * damage > maxPoints)
                {
                    return r;
                }
                r += P(count, count - i, prob);
            }
            return r;
        }

        public static double NormalizeDistribute(double x, int n)
        {
            var sigma = Math.Sqrt(n / 12.0);
            x /= sigma;

            var index = (int)(x * 100);
            if (index >= 310)
            {
                return 0.998;
            }
            else if (index <= -310)
            {
                return 0.002;
            }
            return index < 0 ? 0.5 - _distribution[-index] : 0.5 + _distribution[index];
        }

        public static double Survive(double maxPoints, double minDamage, double maxDamage, float workProb, int count)
        {
            maxPoints -= 1.0;
            if (maxPoints / maxDamage > count)
            {
                return 1.0;
            }
            var avgDamage = (maxDamage + minDamage) / 2;
            var delta = maxDamage - minDamage;

            var r = 0.0;
            for (int i = 0; i <= count; i++)
            {
                var p = P(count, i, 1 - workProb);
                if (i * maxDamage < maxPoints)
                {
                    r += p;
                }
                else if (i * minDamage > maxPoints)
                {
                    break;
                }
                else
                {
                    var x = (maxPoints - i * avgDamage) / delta * 2;
                    var d = NormalizeDistribute(x, i);
                    r += p * d;
                }
            }
            Log.Debug($"{maxPoints}, {minDamage}, {maxDamage}, {workProb}, {count}: {r}");
            return r;
        }
    }
}