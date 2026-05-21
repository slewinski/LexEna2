<?xml version="1.0" encoding="utf-8"?>
<xsl:stylesheet xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:curr="http://www.e-sad.gov.pl/epu" xmlns:fn="http://www.w3.org/2005/xpath-functions" xmlns:link="http://www.xbrl.org/2003/linkbase" xmlns:xbrldi="http://xbrl.org/2006/xbrldi" xmlns:xbrli="http://www.xbrl.org/2003/instance" xmlns:xdt="http://www.w3.org/2005/xpath-datatypes" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:altova="http://www.altova.com" version="1.0" exclude-result-prefixes="curr fn link xbrldi xbrli xdt xlink xs xsd xsi">
  <xsl:output version="4.0" method="html" indent="no" encoding="UTF-8" doctype-public="-//W3C//DTD HTML 4.01 Transitional//EN" doctype-system="http://www.w3.org/TR/html4/loose.dtd" />
  <xsl:param name="SV_OutputFormat" select="'HTML'" />
  <xsl:variable name="XML" select="/" />
  <xsl:decimal-format name="format1" grouping-separator=" " decimal-separator="," />
  <xsl:template match="/">
    <html>
      <head>
        
      </head>
      <body style="margin-left=0.6in; margin-right=0.6in; margin-top=0.79in; margin-bottom=0.79in">
        <xsl:for-each select="$XML">
          <br />
          <div align="right">
            <xsl:for-each select="curr:DokumentEPU">
              <xsl:for-each select="curr:DataZlozenia">
                <span>
                  <xsl:text>Data złożenia </xsl:text>
                </span>
                <span>
                  <xsl:value-of select="format-number(number(substring(string(string(string(.))), 1, 4)), '0000', 'format1')" />
                  <xsl:text>-</xsl:text>
                  <xsl:value-of select="format-number(number(substring(string(string(.)), 6, 2)), '00', 'format1')" />
                  <xsl:text>-</xsl:text>
                  <xsl:value-of select="format-number(number(substring(string(string(.)), 9, 2)), '00', 'format1')" />
                </span>
              </xsl:for-each>
            </xsl:for-each>
          </div>
          <div>
            <xsl:for-each select="curr:DokumentEPU">
              <xsl:for-each select="curr:Sygnatura">
                <span>
                  <xsl:text>Sygnatura akt </xsl:text>
                </span>
                <xsl:apply-templates />
              </xsl:for-each>
            </xsl:for-each>
          </div>
          <br />
          <xsl:for-each select="curr:DokumentEPU">
            <xsl:for-each select="curr:OsobaSkladajaca">
              <span style="text-decoration:underline; ">
                <xsl:text>Osoba składająca</xsl:text>
              </span>
              <br />
              <xsl:for-each select="curr:Osoba">
                <xsl:for-each select="curr:Imie">
                  <xsl:apply-templates />
                  <span>
                    <xsl:text> </xsl:text>
                  </span>
                </xsl:for-each>
                <xsl:for-each select="curr:Nazwisko">
                  <xsl:apply-templates />
                  <span>
                    <xsl:text> </xsl:text>
                  </span>
                </xsl:for-each>
                <xsl:for-each select="curr:PESEL">
                  <span>
                    <xsl:text> PESEL </xsl:text>
                  </span>
                  <xsl:apply-templates />
                </xsl:for-each>
                <br />
                <xsl:for-each select="curr:stanowisko">
                  <xsl:apply-templates />
                </xsl:for-each>
              </xsl:for-each>
              <xsl:for-each select="curr:Nazwa">
                <span>
                  <xsl:text> </xsl:text>
                </span>
                <xsl:apply-templates />
              </xsl:for-each>
              <xsl:for-each select="curr:Adres">
                <xsl:for-each select="@ulica">
                  <span>
                    <xsl:value-of select="string(.)" />
                  </span>
                  <span>
                    <xsl:text> </xsl:text>
                  </span>
                </xsl:for-each>
                <xsl:for-each select="@nr_domu">
                  <span>
                    <xsl:value-of select="string(.)" />
                  </span>
                </xsl:for-each>
                <xsl:for-each select="@nr_mieszkania">
                  <span>
                    <xsl:text>/</xsl:text>
                  </span>
                  <span>
                    <xsl:value-of select="string(.)" />
                  </span>
                </xsl:for-each>
                <xsl:if test="string-length( @kod )!=5">
                  <xsl:for-each select="@kod">
                    <span>
                      <xsl:text> </xsl:text>
                    </span>
                    <span>
                      <xsl:value-of select="string(.)" />
                    </span>
                  </xsl:for-each>
                </xsl:if>
                <xsl:if test="string-length( @kod )=5">
                  <xsl:for-each select="@kod">
                    <span>
                      <xsl:text> </xsl:text>
                    </span>
                    <span>
                      <xsl:value-of select="concat(substring( . , 1 ,2 ),'-',substring( . , 3 , 3 ))" />
                    </span>
                  </xsl:for-each>
                </xsl:if>
                <xsl:for-each select="@miejscowosc">
                  <span>
                    <xsl:text> </xsl:text>
                  </span>
                  <span>
                    <xsl:value-of select="string(.)" />
                  </span>
                </xsl:for-each>
                <xsl:if test="@poczta!=@miejscowosc">
                  <xsl:for-each select="@poczta">
                    <span>
                      <xsl:text> </xsl:text>
                    </span>
                    <span>
                      <xsl:value-of select="string(.)" />
                    </span>
                  </xsl:for-each>
                </xsl:if>
              </xsl:for-each>
            </xsl:for-each>
          </xsl:for-each>
          <br />
          <xsl:if test="curr:DokumentEPU/curr:Rodzaj!=5">
            <span style="text-decoration:underline; ">
              <xsl:text>W imieniu </xsl:text>
            </span>
          </xsl:if>
          <xsl:if test="curr:DokumentEPU/curr:Rodzaj=5">
            <span style="text-decoration:underline; ">
              <xsl:text>Dotyczy</xsl:text>
            </span>
          </xsl:if>
          <br />
          <xsl:for-each select="curr:DokumentEPU">
            <xsl:for-each select="curr:ListaPowodow">
              <xsl:for-each select="curr:Powod">
                <xsl:for-each select="curr:OsobaFizyczna">
                  <xsl:for-each select="curr:Imie">
                    <xsl:apply-templates />
                    <span>
                      <xsl:text> </xsl:text>
                    </span>
                  </xsl:for-each>
                  <xsl:for-each select="curr:Nazwisko">
                    <xsl:apply-templates />
                  </xsl:for-each>
                </xsl:for-each>
                <span>
                  <xsl:text> </xsl:text>
                </span>
                <xsl:for-each select="curr:Instytucja">
                  <xsl:for-each select="curr:Nazwa">
                    <xsl:apply-templates />
                  </xsl:for-each>
                  <xsl:for-each select="curr:Siedziba">
                    <xsl:apply-templates />
                  </xsl:for-each>
                </xsl:for-each>
              </xsl:for-each>
            </xsl:for-each>
          </xsl:for-each>
          <br />
          <xsl:for-each select="curr:DokumentEPU">
            <xsl:for-each select="curr:ListaPozwanych">
              <xsl:for-each select="curr:Pozwany">
                <xsl:for-each select="curr:OsobaFizyczna">
                  <xsl:for-each select="curr:Imie">
                    <xsl:apply-templates />
                    <span>
                      <xsl:text>  </xsl:text>
                    </span>
                  </xsl:for-each>
                  <xsl:for-each select="curr:Nazwisko">
                    <xsl:apply-templates />
                  </xsl:for-each>
                </xsl:for-each>
                <xsl:for-each select="curr:Instytucja">
                  <xsl:for-each select="curr:Nazwa">
                    <xsl:apply-templates />
                  </xsl:for-each>
                  <xsl:for-each select="curr:Siedziba">
                    <xsl:apply-templates />
                  </xsl:for-each>
                </xsl:for-each>
              </xsl:for-each>
            </xsl:for-each>
          </xsl:for-each>
          <br />
          <br />
          <div align="center">
            <xsl:for-each select="curr:DokumentEPU">
              <xsl:if test="curr:Rodzaj=3">
                <xsl:for-each select="curr:Rodzaj">
                  <span style="font-weight:bold; ">
                    <xsl:text>P I S M O </xsl:text>
                  </span>
                </xsl:for-each>
              </xsl:if>
              <xsl:if test="curr:Rodzaj=4">
                <xsl:for-each select="curr:Rodzaj">
                  <span style="font-weight:bold; ">
                    <xsl:text>W N I O S E K</xsl:text>
                  </span>
                </xsl:for-each>
              </xsl:if>
              <xsl:if test="curr:Rodzaj=5">
                <xsl:for-each select="curr:Rodzaj">
                  <span style="font-weight:bold; ">
                    <xsl:text>O D P O W I E D Ź    N A   W E Z W A N I E</xsl:text>
                  </span>
                  <span>
                    <xsl:text>  </xsl:text>
                  </span>
                  <br />
                  <span>
                    <xsl:text> </xsl:text>
                  </span>
                  <span style="font-weight:bold; ">
                    <xsl:text> D O   U Z U P E Ł N  I E N I A  A D R E S U </xsl:text>
                  </span>
                </xsl:for-each>
              </xsl:if>
            </xsl:for-each>
          </div>
          <br />
          <xsl:for-each select="curr:DokumentEPU">
            <xsl:for-each select="curr:Przedmiot">
              <span>
                <xsl:text>Przedmiot: </xsl:text>
              </span>
              <span style="font-weight:bold; ">
                <xsl:apply-templates />
              </span>
              <span>
                <xsl:text> </xsl:text>
              </span>
            </xsl:for-each>
          </xsl:for-each>
          <br />
          <br />
          <xsl:if test="curr:DokumentEPU/curr:Rodzaj=5">
            <xsl:for-each select="curr:DokumentEPU">
              <xsl:for-each select="curr:ListaPozwanych">
                <xsl:for-each select="curr:Pozwany">
                  <span>
                    <xsl:text>Podany adres dotyczy:</xsl:text>
                  </span>
                  <br />
                  <xsl:for-each select="curr:OsobaFizyczna">
                    <xsl:for-each select="curr:Imie">
                      <xsl:apply-templates />
                      <span>
                        <xsl:text>  </xsl:text>
                      </span>
                    </xsl:for-each>
                    <xsl:for-each select="curr:Imie2">
                      <xsl:apply-templates />
                      <span>
                        <xsl:text> </xsl:text>
                      </span>
                    </xsl:for-each>
                    <xsl:for-each select="curr:Nazwisko">
                      <xsl:apply-templates />
                    </xsl:for-each>
                  </xsl:for-each>
                  <xsl:for-each select="curr:Instytucja">
                    <xsl:for-each select="curr:Nazwa">
                      <xsl:apply-templates />
                    </xsl:for-each>
                    <xsl:for-each select="curr:Siedziba">
                      <xsl:apply-templates />
                    </xsl:for-each>
                  </xsl:for-each>
                  <br />
                  <xsl:for-each select="curr:Adres">
                    <br />
                    <xsl:for-each select="@ulica">
                      <span>
                        <xsl:text>Ulica:    </xsl:text>
                      </span>
                      <span>
                        <xsl:value-of select="string(.)" />
                      </span>
                      <span>
                        <xsl:text> </xsl:text>
                      </span>
                    </xsl:for-each>
                    <br />
                    <xsl:for-each select="@nr_domu">
                      <span>
                        <xsl:text>Nr domu:    </xsl:text>
                      </span>
                      <span>
                        <xsl:value-of select="string(.)" />
                      </span>
                    </xsl:for-each>
                    <br />
                    <xsl:for-each select="@nr_mieszkania">
                      <span>
                        <xsl:text>Nr lokalu:    </xsl:text>
                      </span>
                      <span>
                        <xsl:value-of select="string(.)" />
                      </span>
                    </xsl:for-each>
                    <br />
                    <xsl:if test="string-length( @kod )!=5">
                      <xsl:for-each select="@kod">
                        <span>
                          <xsl:text>Kod:    </xsl:text>
                        </span>
                        <span>
                          <xsl:value-of select="string(.)" />
                        </span>
                      </xsl:for-each>
                    </xsl:if>
                    <xsl:if test="string-length( @kod )=5">
                      <xsl:for-each select="@kod">
                        <span>
                          <xsl:text>Kod:   </xsl:text>
                        </span>
                        <span>
                          <xsl:value-of select="concat(substring( . , 1 ,2 ),'-',substring( . , 3 , 3 ))" />
                        </span>
                      </xsl:for-each>
                    </xsl:if>
                    <br />
                    <xsl:for-each select="@miejscowosc">
                      <span>
                        <xsl:text>Miejscowość:    </xsl:text>
                      </span>
                      <span>
                        <xsl:value-of select="string(.)" />
                      </span>
                    </xsl:for-each>
                    <br />
                    <xsl:for-each select="@poczta">
                      <span>
                        <xsl:text>Poczta:    </xsl:text>
                      </span>
                      <span>
                        <xsl:value-of select="string(.)" />
                      </span>
                    </xsl:for-each>
                    <br />
                    <xsl:for-each select="@wojewodztwo">
                      <span>
                        <xsl:text>Województwo:    </xsl:text>
                      </span>
                      <span>
                        <xsl:value-of select="string(.)" />
                      </span>
                    </xsl:for-each>
                  </xsl:for-each>
                </xsl:for-each>
              </xsl:for-each>
            </xsl:for-each>
          </xsl:if>
          <br />
          <pre style="font-family:Times New Roman; font-size:medium; ">
            <div style="font-family:Times New Roman; font-size:medium; " align="justify">
              <xsl:for-each select="curr:DokumentEPU">
                <xsl:for-each select="curr:Tresc">
                  <xsl:apply-templates />
                </xsl:for-each>
              </xsl:for-each>
            </div>
          </pre>
          <br />
          <br />
          <br />
        </xsl:for-each>
      </body>
    </html>
  </xsl:template>
</xsl:stylesheet>