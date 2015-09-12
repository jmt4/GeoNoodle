"""
Given a locality index file, and two letter state
code, this script will parse the file and return state
specific values for cost of living.

Data for locality indices was found here:
https://www.federalpay.org/gs/locality
"""

import sys

def area_pay_adjustment(file, state_code):
    area_pay_adj = {}
    with open(file) as f:
        for line in f.readlines():
            if state_code.upper() in line:
                locality, state_code, city, state, code, pay_adj, year = line.split()
                area_pay_adj[locality] = pay_adj
    return area_pay_adj

if __name__ == '__main__':
    file = sys.argv[1]
    state_code = sys.argv[2]
    pay_adj = area_pay_adjustment(file, state_code)
    print(pay_adj)
