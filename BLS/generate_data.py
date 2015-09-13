import sys
from os import path
sys.path.append(path.dirname(path.dirname(path.abspath(__file__))))
#import urllib.request
import qcew
"""
The point of this script is to use the extended BLS API
to gather data, format data, and write to a file for
charting. Specifically, industry wage, average wage, and
median wage with respect to time will be computed.
"""

try:
    start, end, state, industry = sys.argv[1:]
except ValueError:
    print("Supply start, end, state, industry arguments only.")

avg_wage = []
median_wage = []

for year in range(int(start), int(end) + 1, 1):
    print(year)
    '''
        We want to iterate through the year range and compile
        the industry avg weekly wage for supplied industry.
        We need to tie this to the year and eventually the industry,
        and finally this needs to be written in JSON for highcharts.
        A similar process should be done for the methods below.
    '''
    avg_wage.append((year, qcew.qcewGetAvgWageByAreaAndYear("%s" % (year,), "1", "04000")))
    median_wage.append((year, qcew.qcewGetMedianWageByAreaAndYear("%s" % (year,), "1", "04000")))
    #gcewGetIndustryAreaData(year, qtr, industry, state)
    #qcewGetAvgWageByAreaAndYear(year, qtr, area)
    #qcewGetMedianWageByAreaAndYear(year, qtr, area)
