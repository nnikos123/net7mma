﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace nVideo.Common.DCT
{
    public class SparseIDCT
    {

        public static int[][] COEFF = new int[64][];
        public static int PRECISION = 13;
        public static int DC_SHIFT = PRECISION - 3;

        static SparseIDCT()
        {
            COEFF[0] = new int[64];
            Arrays.fill(COEFF[0], 1 << DC_SHIFT);

            int ac = 1 << PRECISION;
            for (int i = 1; i < 64; i++)
            {
                COEFF[i] = new int[64];
                COEFF[i][i] = ac;
                SimpleIDCT10Bit.idct10(COEFF[i], 0);
            }
        }

        /**
         * Starts DCT reconstruction
         * 
         * Faster then call to 'coeff' with ind = 0
         * 
         * @param block
         * @param dc
         */
        public static void start(int[] block, int dc)
        {
            dc <<= DC_SHIFT;
            for (int i = 0; i < 64; i += 4)
            {
                block[i + 0] = dc;
                block[i + 1] = dc;
                block[i + 2] = dc;
                block[i + 3] = dc;
            }
        }

        /**
         * Recalculates image based on new DCT coefficient
         * 
         * @param block
         * @param ind
         * @param level
         */
        public static void coeff(int[] block, int ind, int level)
        {
            for (int i = 0; i < 64; i += 4)
            {
                block[i] += COEFF[ind][i] * level;
                block[i + 1] += COEFF[ind][i + 1] * level;
                block[i + 2] += COEFF[ind][i + 2] * level;
                block[i + 3] += COEFF[ind][i + 3] * level;
            }
        }

        /**
         * izes DCT calculation
         * 
         * @param block
         */
        public static void finish(int[] block)
        {
            for (int i = 0; i < 64; i += 4)
            {
                block[i] = div(block[i]);
                block[i + 1] = div(block[i + 1]);
                block[i + 2] = div(block[i + 2]);
                block[i + 3] = div(block[i + 3]);
            }
        }

        private static int div(int x)
        {
            int m = x >> 31;
            int n = x >> 31;
            return ((((x ^ m) + n) >> PRECISION) ^ m) + n;
        }
    }
}
