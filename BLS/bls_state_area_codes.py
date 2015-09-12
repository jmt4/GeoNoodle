"""
Given the name of a state, return the areas
in that state and their corresponding codes.

Source:
http://www.bls.gov/cew/doc/titles/area/area_titles.htm
"""

import sys
STATES_ABBREV = {
'ALABAMA': 'AL',
'ALASKA': 'AK',
'ARIZONA': 'AZ',
'ARKANSAS': 'AR',
'CALIFORNIA': 'CA',
'COLORADO': 'CO',
'CONNECTICUT': 'CT',
'DELAWARE': 'DE',
'FLORIDA': 'FL',
'GEORGIA': 'GA',
'HAWAII': 'HI',
'IDAHO': 'ID',
'ILLINOIS': 'IL',
'INDIANA': 'IN',
'IOWA': 'IA',
'KANSAS': 'KS',
'KENTUCKY': 'KY',
'LOUISIANA': 'LA',
'MAINE': 'ME',
'MARYLAND': 'MD',
'MASSACHUSETTS': 'MA',
'MICHIGAN': 'MI',
'MINNESOTA': 'MN',
'MISSISSIPPI': 'MS',
'MISSOURI': 'MO',
'MONTANA': 'MT',
'NEBRASKA': 'NE',
'NEVADA': 'NV',
'NEW HAMPSHIRE': 'NH',
'NEW JERSEY': 'NJ',
'NEW MEXICO': 'NM',
'NEW YORK': 'NY',
'NORTH CAROLINA': 'NC',
'NORTH DAKOTA': 'ND',
'OHIO': 'OH',
'OKLAHOMA': 'OK',
'OREGON': 'OR',
'PENNSYLVANIA': 'PA',
'RHODE ISLAND': 'RI',
'SOUTH CAROLINA': 'SC',
'SOUTH DAKOTA': 'SD',
'TENNESSEE': 'TN',
'TEXAS': 'TX',
'UTAH': 'UT',
'VERMONT': 'VT',
'VIRGINIA': 'VA',
'WASHINGTON': 'WA',
'WEST VIRGINIA': 'WV',
'WISCONSIN': 'WI',
'WYOMING': 'WY'
}

def get_area_codes_by_state(file, state):
    code_area = {}
    with open(file) as f:
        us_total = f.readline()
        us_comb = f.readline()
        us_metro = f.readline()
        us_nonmetro = f.readline()
        for line in f.readlines():
            if state in line or STATES_ABBREV[state.upper()] in line:
                if len(line.split()) == 4:
                    """
                     Unpacking values of the following form
                     '04007	Gila County, Arizona'
                    """
                    code, county_name, x, y = line.split()
                    code_area[code] = "%s %s" % (county_name.replace(',', ''), "County",)
    return code_area

if __name__ == '__main__':
    file = sys.argv[1]
    state = sys.argv[2]
    code_area = get_area_codes_by_state(file, state)
    print(code_area)
