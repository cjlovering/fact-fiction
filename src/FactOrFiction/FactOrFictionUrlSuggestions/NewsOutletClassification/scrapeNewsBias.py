import urllib2
from bs4 import BeautifulSoup
import re
import csv
import string

printable = set(string.printable)
ignore = "IGNORE"


def writeLine(url, bias):
    results = ""
    page = urllib2.urlopen(url)
    soup = BeautifulSoup(page, 'html.parser')
    paragraphs = soup.find('div', attrs={'class':'entry clearfix'}).findAll('p')
    #factual = getTextWithLeading(paragraphs, 'Factual Reporting:').encode('utf-8').strip()
    factual = "N/A"
    notes = stripWeirdChars(getTextWithLeading(paragraphs, 'Notes:').encode('utf-8').strip().replace(',', ';'))
    source = getTextWithLeading(paragraphs, 'Source:').encode('utf-8').strip()
    if (factual == ignore or notes == ignore or source == ignore):
        print url + " was ignored"
        return
    with open('index.csv', 'a') as csv_file:
        writer = csv.writer(csv_file)
        writer.writerow([source, factual, bias, notes])

def stripWeirdChars(s):
    return filter(lambda x: x in printable, s)

def getTextWithLeading(paragraphs, leading):
    for paragraph in paragraphs:
        if leading in paragraph.text: 
           return paragraph.text.split(leading)[1].strip()
    return ignore

def getAllUrls(url, bias):
    page = urllib2.urlopen(url)
    soup = BeautifulSoup(page, 'html.parser')
    links = soup.find('div', attrs={'class':'entry clearfix'}).findAll('a', attrs={'href': re.compile("^http://")})
    for link in links:
        #print link['href']
        writeLine(link['href'], bias)
    

url = 'https://mediabiasfactcheck.com/fake-news/'
getAllUrls(url, 'Questionable Sources')
#writeLine('https://mediabiasfactcheck.com/all-thats-fab/', 'Left Bias')