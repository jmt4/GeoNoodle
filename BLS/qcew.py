# U.S. Department of Labor
# Bureau of Labor Statistics 
# Quarterly Census of Employment and Wages
# July 2014
#  
# QCEW Open Data Access for Python 2.x
#  
# This version was written using Python 2.7 and should work with other "2.x"
# versions. However, some modification may be needed. There is a separate 
# example file for "3.x" versions Python. 
#
#
# Submit questions at: http://data.bls.gov/cgi-bin/forms/cew?/cew/home.htm 
#
# *******************************************************************************
# September 2015
# Converted to python 3.x. Extended API to include some more finely-grained data
# retrieval.
#
# ********************************************************************************


import urllib.request
import statistics
from bls_state_area_codes import get_state_area_codes


# *******************************************************************************
# qcewCreateDataRows : This function takes a raw csv string and splits it into
# a two-dimensional array containing the data and the header row of the csv file
def qcewCreateDataRows(csv):
    dataLines = csv.split(b'\r\n')
    dataRows = []
    for row in dataLines:
        dataRows.append(row.split(b','))
    return dataRows
# *******************************************************************************




# *******************************************************************************
# qcewGetAreaData : This function takes a year, quarter, and area argument and
# returns an array containing the associated area data. Use 'a' for annual
# averages. 
# For all area codes and titles see:
# http://www.bls.gov/cew/doc/titles/area/area_titles.htm
#
def qcewGetAreaData(year,qtr,area):
    urlPath = "http://www.bls.gov/cew/data/api/[YEAR]/[QTR]/area/[AREA].csv"
    urlPath = urlPath.replace("[YEAR]",year)
    urlPath = urlPath.replace("[QTR]",qtr)
    urlPath = urlPath.replace("[AREA]",area)
    httpStream = urllib.request.urlopen(urlPath)
    csv = httpStream.read()
    httpStream.close()
    return qcewCreateDataRows(csv)
# *******************************************************************************




# *******************************************************************************
# qcewGetIndustryData : This function takes a year, quarter, and industry code
# and returns an array containing the associated industry data. Use 'a' for 
# annual averages. Some industry codes contain hyphens. The CSV files use
# underscores instead of hyphens. So 31-33 becomes 31_33. 
# For all industry codes and titles see:
# http://www.bls.gov/cew/doc/titles/industry/industry_titles.htm
#
def qcewGetIndustryData(year,qtr,industry):
    urlPath = "http://www.bls.gov/cew/data/api/[YEAR]/[QTR]/industry/[IND].csv"
    urlPath = urlPath.replace("[YEAR]",year)
    urlPath = urlPath.replace("[QTR]",qtr)
    urlPath = urlPath.replace("[IND]",industry)
    httpStream = urllib.request.urlopen(urlPath)
    csv = httpStream.read()
    httpStream.close()
    return qcewCreateDataRows(csv)
# *******************************************************************************





# *******************************************************************************
# qcewGetSizeData : This function takes a year and establishment size class code
# and returns an array containing the associated size data. Size data
# is only available for the first quarter of each year.
# For all establishment size classes and titles see:
# http://www.bls.gov/cew/doc/titles/size/size_titles.htm
#
def qcewGetSizeData(year,size):
    urlPath = "http://www.bls.gov/cew/data/api/[YEAR]/1/size/[SIZE].csv"
    urlPath = urlPath.replace("[YEAR]",year)
    urlPath = urlPath.replace("[SIZE]",size)
    httpStream = urllib.request.urlopen(urlPath)
    csv = httpStream.read()
    httpStream.close()
    return qcewCreateDataRows(csv)
# *******************************************************************************


# *******************************************************************************
# qcewGetIndustryAreaData : This function takes a year, quarter, industry code,
# and area code and returns a list of information from that quarter. Specifically,
# industry data for each area (county, city) in a state is returned as a list of
# a lists.
# *******************************************************************************
#
def gcewGetIndustryAreaData(year, qtr, industry, state):
    urlPath = "http://www.bls.gov/cew/data/api/[YEAR]/[QTR]/industry/[IND].csv"
    urlPath = urlPath.replace("[YEAR]",year)
    urlPath = urlPath.replace("[QTR]",qtr)
    urlPath = urlPath.replace("[IND]",industry)
    httpStream = urllib.request.urlopen(urlPath)
    csv = httpStream.read()
    httpStream.close()
    state_area_codes = get_state_area_codes('code area.txt', state)
    def get_area_by_code(dataLines, area):
        for row in dataLines:
            if area in row:
                return row.split(b',')
    data = []
    lines = csv.split(b'\r\n')
    for code in state_area_codes:
        area = get_area_by_code(lines, code)
        if area:
            data.append(area)
    return data
# *******************************************************************************

# *******************************************************************************
# qcewGetAvgWageByAreaAndYear : This function returns the average weekly wage of
# a specific area given that area, the year, and quarter. Specifically, we simply
# sum up the avg weekly wage for each industry and divide by the number of
# industries.
# *******************************************************************************
#
def qcewGetAvgWageByAreaAndYear(year, qtr, area):
    urlPath = "http://www.bls.gov/cew/data/api/[YEAR]/[QTR]/area/[AREA].csv"
    urlPath = urlPath.replace("[YEAR]",year)
    urlPath = urlPath.replace("[QTR]",qtr)
    urlPath = urlPath.replace("[AREA]",area)
    httpStream = urllib.request.urlopen(urlPath)
    csv = httpStream.read()
    httpStream.close()
    lines = csv.split(b'\r\n')[1:]
    running_total = 0;
    for idx, line in enumerate(lines):
        line = line.decode()
        line = line.split(',')
        try:
            running_total += int(line[15])
        except IndexError:
            break
    return running_total/(float(len(lines)))
# *******************************************************************************

# *******************************************************************************
# qcewGetMedianWageByAreaAndYear : This function returns the area-wide median weekly
# wage given a year, quarter, and area. Specifically, we create a list and append
# industry weekly wages then use statistics package to calculate median.
# *******************************************************************************
#
def qcewGetMedianWageByAreaAndYear(year, qtr, area):
    urlPath = "http://www.bls.gov/cew/data/api/[YEAR]/[QTR]/area/[AREA].csv"
    urlPath = urlPath.replace("[YEAR]",year)
    urlPath = urlPath.replace("[QTR]",qtr)
    urlPath = urlPath.replace("[AREA]",area)
    httpStream = urllib.request.urlopen(urlPath)
    csv = httpStream.read()
    httpStream.close()
    lines = csv.split(b'\r\n')[1:]
    running_total = [];
    for idx, line in enumerate(lines):
        line = line.decode()
        line = line.split(',')
        try:
            running_total.append(int(line[15]))
        except IndexError:
            break
    return statistics.median(running_total)
# *******************************************************************************

Michigan_Data = qcewGetAreaData("2013","1","26000")
Auto_Manufacturing = qcewGetIndustryData("2013","1","3361")
SizeData = qcewGetSizeData("2013","6")

if __name__ == '__main__':
    Arizona_Avg_Wage = qcewGetAvgWageByAreaAndYear("2012", "1", "04000")
    Arizona_Median_Wage = qcewGetMedianWageByAreaAndYear("2012", "1", "04000")
    print(Arizona_Avg_Wage)
    print(Arizona_Median_Wage)





