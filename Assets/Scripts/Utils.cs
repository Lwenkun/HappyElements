using System;
using System.Collections.Generic;

public class Utils {

    int[] SplitNumber(int num) {

        List<int> list = new List<int>();

        while (num != 0)
        {
            list.Add(num % 10);
            num /= 10;
        }

        return list.ToArray();
    }
}

