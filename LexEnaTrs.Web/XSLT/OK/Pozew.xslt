<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:curr="http://www.e-sad.gov.pl/epu" xmlns:fn="http://www.w3.org/2005/xpath-functions" xmlns:link="http://www.xbrl.org/2003/linkbase" xmlns:xbrldi="http://xbrl.org/2006/xbrldi" xmlns:xbrli="http://www.xbrl.org/2003/instance" xmlns:xdt="http://www.w3.org/2005/xpath-datatypes" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:altova="http://www.altova.com" exclude-result-prefixes="curr fn link xbrldi xbrli xdt xlink xs xsd xsi">
	<xsl:output version="4.0" method="html" indent="no" encoding="UTF-8" doctype-public="-//W3C//DTD HTML 4.01 Transitional//EN" doctype-system="http://www.w3.org/TR/html4/loose.dtd"/>
	<xsl:param name="SV_OutputFormat" select="'HTML'"/>
	<xsl:variable name="XML" select="/"/>
	<xsl:template match="/">
		<html>
			<head>
				<meta http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
			</head>
			<body  style="margin-left=0.6in; margin-right=0.6in; margin-top=0.79in; margin-bottom=0.79in">
				<xsl:for-each select="$XML">
					<br/>
					<div align="left">
						<xsl:for-each select="curr:PozewEPU">
							<xsl:for-each select="@dataZlozenia">
								<span>
									<xsl:text>Data ostatniej modyfikacji </xsl:text>
								</span>
								<span>
									<xsl:value-of select="string(.)"/>
								</span>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<div>
						<xsl:for-each select="curr:PozewEPU">
							<xsl:for-each select="curr:SprawaWgPowoda">
								<span>
									<xsl:text>Oznaczenie sprawy wg składającego </xsl:text>
								</span>
								<xsl:apply-templates/>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<div align="right">
						<span>
							<xsl:text>&#160;</xsl:text>
						</span>
						<xsl:for-each select="curr:PozewEPU">
							<xsl:for-each select="curr:Adresat">
								<xsl:for-each select="curr:Nazwa">
									<xsl:apply-templates/>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<div align="right">
						<xsl:for-each select="curr:PozewEPU">
							<xsl:for-each select="curr:Adresat">
								<xsl:for-each select="curr:Wydzial">
									<xsl:apply-templates/>
								</xsl:for-each>
								<br/>
								<xsl:for-each select="curr:Adres">
									<xsl:for-each select="@ulica">
										<span>
											<xsl:value-of select="string(.)"/>
										</span>
									</xsl:for-each>
									<xsl:for-each select="@nr_domu">
										<span>
											<xsl:text>&#160;</xsl:text>
										</span>
										<span>
											<xsl:value-of select="string(.)"/>
										</span>
									</xsl:for-each>
									<xsl:if test="name(@nr_mieszkania) and  name(@nr_mieszkania)!=&apos;&apos; and string-length( normalize-space( @nr_mieszkania)) &gt;0">
										<xsl:for-each select="@nr_mieszkania">
											<span>
												<xsl:text>/</xsl:text>
											</span>
											<span>
												<xsl:value-of select="string(.)"/>
											</span>
										</xsl:for-each>
									</xsl:if>
									<br/>
									<xsl:if test="string-length( @kod )!=5">
										<xsl:for-each select="@kod">
											<span>
												<xsl:text>&#160;</xsl:text>
											</span>
											<span>
												<xsl:value-of select="string(.)"/>
											</span>
										</xsl:for-each>
									</xsl:if>
									<xsl:if test="string-length( @kod )=5">
										<xsl:for-each select="@kod">
											<span>
												<xsl:text>&#160;</xsl:text>
											</span>
											<span>
												<xsl:value-of select="concat(substring( . , 1,2 ),&apos;-&apos;,substring( . , 3,3 ))"/>
											</span>
										</xsl:for-each>
									</xsl:if>
									<xsl:for-each select="@miejscowosc">
										<span>
											<xsl:text>&#160;</xsl:text>
										</span>
										<span>
											<xsl:value-of select="string(.)"/>
										</span>
									</xsl:for-each>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<div>
						<span style="text-decoration:underline; ">
							<xsl:text>Osoba składająca</xsl:text>
						</span>
					</div>
					<div>
						<xsl:for-each select="curr:PozewEPU">
							<xsl:for-each select="curr:OsobaSkladajaca">
								<xsl:for-each select="curr:Osoba">
									<xsl:for-each select="curr:Imie">
										<xsl:apply-templates/>
										<span>
											<xsl:text>&#160;</xsl:text>
										</span>
									</xsl:for-each>
									<xsl:for-each select="curr:Imie2">
										<xsl:apply-templates/>
										<span>
											<xsl:text>&#160;</xsl:text>
										</span>
									</xsl:for-each>
									<xsl:for-each select="curr:Nazwisko">
										<xsl:apply-templates/>
									</xsl:for-each>
									<span>
										<xsl:text>&#160;</xsl:text>
									</span>
									<xsl:for-each select="curr:PESEL">
										<span>
											<xsl:text>&#160;&#160; PESEL </xsl:text>
										</span>
										<xsl:apply-templates/>
									</xsl:for-each>
									<br/>
									<xsl:for-each select="curr:stanowisko">
										<xsl:apply-templates/>
									</xsl:for-each>
								</xsl:for-each>
								<xsl:for-each select="curr:Nazwa">
									<xsl:apply-templates/>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<div>
						<xsl:for-each select="curr:PozewEPU">
							<xsl:for-each select="curr:OsobaSkladajaca">
								<xsl:for-each select="curr:Adres">
									<xsl:for-each select="@ulica">
										<span>
											<xsl:value-of select="string(.)"/>
										</span>
										<span>
											<xsl:text>&#160;</xsl:text>
										</span>
									</xsl:for-each>
									<xsl:if test="string-length(@ulica) &lt; 3">
										<xsl:for-each select="@miejscowosc">
											<span>
												<xsl:value-of select="string(.)"/>
											</span>
											<span>
												<xsl:text>&#160;</xsl:text>
											</span>
										</xsl:for-each>
									</xsl:if>
									<xsl:for-each select="@nr_domu">
										<span>
											<xsl:value-of select="string(.)"/>
										</span>
										<span>
											<xsl:text>&#160;</xsl:text>
										</span>
									</xsl:for-each>
									<span>
										<xsl:text>&#160;</xsl:text>
									</span>
									<xsl:if test="string-length( @nr_mieszkania )&gt;0">
										<xsl:if test="name(@nr_mieszkania) and  name(@nr_mieszkania)!=&apos;&apos; and string-length( normalize-space( @nr_mieszkania)) &gt;0">
											<xsl:for-each select="@nr_mieszkania">
												<span>
													<xsl:text>/</xsl:text>
												</span>
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
											</xsl:for-each>
										</xsl:if>
									</xsl:if>
									<xsl:if test="string-length( @kod )!=5">
										<xsl:for-each select="@kod">
											<span>
												<xsl:text>&#160;</xsl:text>
											</span>
											<span>
												<xsl:value-of select="string(.)"/>
											</span>
										</xsl:for-each>
									</xsl:if>
									<span>
										<xsl:text>&#160;</xsl:text>
									</span>
									<xsl:if test="string-length( @kod )=5">
										<xsl:for-each select="@kod">
											<span>
												<xsl:text>&#160;</xsl:text>
											</span>
											<span>
												<xsl:value-of select="concat( substring( . ,1, 2 ),&apos;-&apos; ,substring( . , 3 ,3 ) )"/>
											</span>
										</xsl:for-each>
									</xsl:if>
									<xsl:if test="string-length(@ulica)&gt;2">
										<xsl:for-each select="@miejscowosc">
											<span>
												<xsl:text>&#160;</xsl:text>
											</span>
											<span>
												<xsl:value-of select="string(.)"/>
											</span>
										</xsl:for-each>
									</xsl:if>
									<span>
										<xsl:text>&#160;</xsl:text>
									</span>
									<xsl:if test="@poczta!=@miejscowosc or string-length( @ulica )&lt;3">
										<xsl:for-each select="@poczta">
											<span>
												<xsl:text>&#160;</xsl:text>
											</span>
											<span>
												<xsl:value-of select="string(.)"/>
											</span>
										</xsl:for-each>
									</xsl:if>
								</xsl:for-each>
								<xsl:for-each select="@podstawa">
									<br/>
									<span>
										<xsl:value-of select="string(.)"/>
									</span>
									<br/>
								</xsl:for-each>
								<xsl:for-each select="$XML">
									<xsl:for-each select="curr:PozewEPU">
										<xsl:for-each select="curr:ListaPowodow">
											<xsl:if test="( curr:Powod/curr:reprezentacja=1 ) or ( curr:Powod/curr:reprezentacja=3 ) or ( curr:Powod/curr:reprezentacja=4 )">
												<span style="text-decoration:underline; ">
													<xsl:text>W imieniu: </xsl:text>
												</span>
											</xsl:if>
											<xsl:for-each select="curr:Powod[curr:reprezentacja!=0 and curr:reprezentacja!=2]">
												<xsl:for-each select="curr:OsobaFizyczna">
													<xsl:for-each select="curr:Imie">
														<br/>
														<xsl:apply-templates/>
													</xsl:for-each>
													<xsl:for-each select="curr:Imie2">
														<span>
															<xsl:text>&#160;</xsl:text>
														</span>
														<xsl:apply-templates/>
													</xsl:for-each>
													<xsl:for-each select="curr:Nazwisko">
														<span>
															<xsl:text>&#160;</xsl:text>
														</span>
														<xsl:apply-templates/>
													</xsl:for-each>
												</xsl:for-each>
												<xsl:for-each select="curr:Instytucja">
													<xsl:for-each select="curr:Nazwa">
														<xsl:apply-templates/>
													</xsl:for-each>
													<xsl:if test="contains( curr:Siedziba , &apos;z siedzibą&apos; )=false">
														<xsl:for-each select="curr:Siedziba">
															<span>
																<xsl:text> z siedzibą w </xsl:text>
															</span>
														</xsl:for-each>
													</xsl:if>
													<xsl:for-each select="curr:Siedziba">
														<span>
															<xsl:text>&#160;</xsl:text>
														</span>
														<xsl:apply-templates/>
													</xsl:for-each>
												</xsl:for-each>
												<xsl:if test="curr:reprezentacja=1">
													<xsl:for-each select="curr:reprezentacja">
														<span>
															<xsl:text> (pełnomocnik)</xsl:text>
														</span>
													</xsl:for-each>
												</xsl:if>
												<xsl:if test="curr:reprezentacja=3">
													<xsl:for-each select="curr:reprezentacja">
														<span>
															<xsl:text> (przedstawiciel ustawowy)</xsl:text>
														</span>
													</xsl:for-each>
												</xsl:if>
												<xsl:if test="curr:reprezentacja=4">
													<xsl:for-each select="curr:reprezentacja">
														<span>
															<xsl:text> (osoba reprezentująca) </xsl:text>
														</span>
													</xsl:for-each>
												</xsl:if>
											</xsl:for-each>
										</xsl:for-each>
									</xsl:for-each>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<br/>
					<xsl:if test="count( curr:PozewEPU/curr:ListaPowodow/curr:Powod )&gt;1">
						<div>
							<span style="text-decoration:underline; ">
								<xsl:text>Powodowie</xsl:text>
							</span>
						</div>
					</xsl:if>
					<xsl:if test="count( curr:PozewEPU/curr:ListaPowodow/curr:Powod )&lt;=1">
						<div>
							<span style="text-decoration:underline; ">
								<xsl:text>Powód</xsl:text>
							</span>
						</div>
					</xsl:if>
					<div>
						<xsl:for-each select="curr:PozewEPU">
							<xsl:for-each select="curr:ListaPowodow">
								<xsl:for-each select="curr:Powod">
									<xsl:for-each select="curr:OsobaFizyczna">
										<br/>
										<xsl:for-each select="curr:Imie">
											<span style="font-weight:bold; ">
												<xsl:apply-templates/>
											</span>
										</xsl:for-each>
										<xsl:for-each select="curr:Imie2">
											<span style="font-weight:bold; ">
												<xsl:text>&#160;</xsl:text>
											</span>
											<span style="font-weight:bold; ">
												<xsl:apply-templates/>
											</span>
										</xsl:for-each>
										<xsl:for-each select="curr:Nazwisko">
											<span>
												<xsl:text>&#160;</xsl:text>
											</span>
											<span style="font-weight:bold; ">
												<xsl:apply-templates/>
											</span>
										</xsl:for-each>
										<xsl:for-each select="curr:Nazwa">
											<span>
												<xsl:text>&#160;</xsl:text>
											</span>
											<xsl:apply-templates/>
										</xsl:for-each>
										<xsl:for-each select="curr:PESEL">
											<span>
												<xsl:text> PESEL </xsl:text>
											</span>
											<xsl:apply-templates/>
										</xsl:for-each>
									</xsl:for-each>
									<xsl:for-each select="curr:Instytucja">
										<xsl:for-each select="curr:Nazwa">
											<span style="font-weight:bold; ">
												<xsl:apply-templates/>
											</span>
										</xsl:for-each>
										<xsl:if test="contains( curr:Siedziba , &apos;z siedzibą&apos; )=false">
											<xsl:for-each select="curr:Siedziba">
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
												<span style="font-weight:bold; ">
													<xsl:text>z siedzibą w</xsl:text>
												</span>
											</xsl:for-each>
										</xsl:if>
										<xsl:for-each select="curr:Siedziba">
											<span>
												<xsl:text>&#160;</xsl:text>
											</span>
											<span style="font-weight:bold; ">
												<xsl:apply-templates/>
											</span>
										</xsl:for-each>
										<xsl:for-each select="curr:KRS">
											<span>
												<xsl:text> KRS </xsl:text>
											</span>
											<xsl:apply-templates/>
										</xsl:for-each>
										<xsl:for-each select="curr:InnyRejestr">
											<xsl:for-each select="@typ">
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
											</xsl:for-each>
											<xsl:for-each select="@organ">
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
											</xsl:for-each>
											<xsl:for-each select="@numer">
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
											</xsl:for-each>
										</xsl:for-each>
										<xsl:for-each select="curr:REGON">
											<span>
												<xsl:text> REGON </xsl:text>
											</span>
											<xsl:apply-templates/>
										</xsl:for-each>
									</xsl:for-each>
									<xsl:for-each select="curr:NIP">
										<span>
											<xsl:text> NIP </xsl:text>
										</span>
										<xsl:apply-templates/>
									</xsl:for-each>
									<xsl:for-each select="curr:Adres">
										<xsl:for-each select="@ulica">
											<br/>
											<span>
												<xsl:value-of select="string(.)"/>
											</span>
										</xsl:for-each>
										<xsl:if test="string-length(@ulica) &lt; 3">
											<xsl:for-each select="@miejscowosc">
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
											</xsl:for-each>
										</xsl:if>
										<xsl:for-each select="@nr_domu">
											<span>
												<xsl:text>&#160;</xsl:text>
											</span>
											<span>
												<xsl:value-of select="string(.)"/>
											</span>
										</xsl:for-each>
										<xsl:if test="name(@nr_mieszkania) and  name(@nr_mieszkania)!=&apos;&apos; and string-length( normalize-space( @nr_mieszkania)) &gt;0">
											<xsl:for-each select="@nr_mieszkania">
												<span>
													<xsl:text>/</xsl:text>
												</span>
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
											</xsl:for-each>
										</xsl:if>
										<xsl:if test="string-length( @kod )!=5">
											<xsl:for-each select="@kod">
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
											</xsl:for-each>
										</xsl:if>
										<xsl:if test="string-length( @kod )=5">
											<xsl:for-each select="@kod">
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
												<span>
													<xsl:value-of select="concat(substring( . ,1, 2 ) ,&apos;-&apos;, substring( . , 3 , 3 ) )"/>
												</span>
											</xsl:for-each>
										</xsl:if>
										<xsl:if test="string-length(@ulica)&gt;2">
											<xsl:for-each select="@miejscowosc">
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
											</xsl:for-each>
										</xsl:if>
										<xsl:if test="@poczta!=@miejscowosc or string-length( @ulica )&lt;3">
											<xsl:for-each select="@poczta">
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
											</xsl:for-each>
										</xsl:if>
									</xsl:for-each>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<br/>
					<xsl:if test="count( curr:PozewEPU/curr:ListaPozwanych/curr:Pozwany )&gt;1">
						<div>
							<span style="text-decoration:underline; ">
								<xsl:text>Pozwani</xsl:text>
							</span>
						</div>
					</xsl:if>
					<xsl:if test="count( curr:PozewEPU/curr:ListaPozwanych/curr:Pozwany )&lt;=1">
						<div>
							<span style="text-decoration:underline; ">
								<xsl:text>Pozwany</xsl:text>
							</span>
						</div>
					</xsl:if>
					<div>
						<xsl:for-each select="curr:PozewEPU">
							<xsl:for-each select="curr:ListaPozwanych">
								<xsl:for-each select="curr:Pozwany">
									<xsl:for-each select="curr:OsobaFizyczna">
										<br/>
										<xsl:for-each select="curr:Imie">
											<span style="font-weight:bold; ">
												<xsl:apply-templates/>
											</span>
										</xsl:for-each>
										<xsl:for-each select="curr:Imie2">
											<span>
												<xsl:text>&#160;</xsl:text>
											</span>
											<span style="font-weight:bold; ">
												<xsl:apply-templates/>
											</span>
										</xsl:for-each>
										<xsl:for-each select="curr:Nazwisko">
											<span>
												<xsl:text>&#160;</xsl:text>
											</span>
											<span style="font-weight:bold; ">
												<xsl:apply-templates/>
											</span>
										</xsl:for-each>
										<xsl:for-each select="curr:Nazwa">
											<span>
												<xsl:text>&#160;</xsl:text>
											</span>
											<xsl:apply-templates/>
										</xsl:for-each>
										<xsl:for-each select="curr:PESEL">
											<span>
												<xsl:text> PESEL </xsl:text>
											</span>
											<xsl:apply-templates/>
										</xsl:for-each>
									</xsl:for-each>
									<xsl:for-each select="curr:Instytucja">
										<xsl:for-each select="curr:Nazwa">
											<span style="font-weight:bold; ">
												<xsl:apply-templates/>
											</span>
										</xsl:for-each>
										<xsl:if test="contains( curr:Siedziba , &apos;z siedzibą&apos; )=false">
											<xsl:for-each select="curr:Siedziba">
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
												<span style="font-weight:bold; ">
													<xsl:text>z siedzibą w</xsl:text>
												</span>
											</xsl:for-each>
										</xsl:if>
										<xsl:for-each select="curr:Siedziba">
											<span style="font-weight:bold; ">
												<xsl:text>&#160;</xsl:text>
											</span>
											<span style="font-weight:bold; ">
												<xsl:apply-templates/>
											</span>
										</xsl:for-each>
										<xsl:for-each select="curr:KRS">
											<span>
												<xsl:text> KRS </xsl:text>
											</span>
											<xsl:apply-templates/>
										</xsl:for-each>
										<xsl:for-each select="curr:InnyRejestr">
											<xsl:for-each select="@typ">
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
											</xsl:for-each>
											<xsl:for-each select="@organ">
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
											</xsl:for-each>
											<xsl:for-each select="@numer">
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
											</xsl:for-each>
										</xsl:for-each>
										<xsl:for-each select="curr:REGON">
											<span>
												<xsl:text> REGON </xsl:text>
											</span>
											<xsl:apply-templates/>
										</xsl:for-each>
									</xsl:for-each>
									<xsl:for-each select="curr:NIP">
										<span>
											<xsl:text> NIP </xsl:text>
										</span>
										<xsl:apply-templates/>
									</xsl:for-each>
									<xsl:for-each select="curr:Adres">
										<xsl:for-each select="@ulica">
											<br/>
											<span>
												<xsl:value-of select="string(.)"/>
											</span>
										</xsl:for-each>
										<xsl:if test="string-length(@ulica) &lt; 3">
											<xsl:for-each select="@miejscowosc">
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
											</xsl:for-each>
										</xsl:if>
										<xsl:for-each select="@nr_domu">
											<span>
												<xsl:text>&#160;</xsl:text>
											</span>
											<span>
												<xsl:value-of select="string(.)"/>
											</span>
										</xsl:for-each>
										<xsl:if test="name(@nr_mieszkania) and  name(@nr_mieszkania)!=&apos;&apos; and string-length( normalize-space( @nr_mieszkania)) &gt;0">
											<xsl:for-each select="@nr_mieszkania">
												<span>
													<xsl:text>/</xsl:text>
												</span>
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
											</xsl:for-each>
										</xsl:if>
										<xsl:if test="string-length( @kod )!=5">
											<xsl:for-each select="@kod">
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
											</xsl:for-each>
										</xsl:if>
										<xsl:if test="string-length( @kod )=5">
											<xsl:for-each select="@kod">
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
												<span>
													<xsl:value-of select="concat( substring( . ,1, 2 ) ,&apos;-&apos;, substring( . , 3 , 3 ) )"/>
												</span>
											</xsl:for-each>
										</xsl:if>
										<xsl:if test="string-length(@ulica)&gt;2">
											<xsl:for-each select="@miejscowosc">
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
											</xsl:for-each>
										</xsl:if>
										<xsl:if test="@poczta!=@miejscowosc or string-length( @ulica )&lt;3">
											<xsl:for-each select="@poczta">
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
												<span>
													<xsl:value-of select="string(.)"/>
												</span>
											</xsl:for-each>
										</xsl:if>
									</xsl:for-each>
								</xsl:for-each>
								<br/>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<br/>
					<div>
						<xsl:for-each select="curr:PozewEPU">
							<xsl:for-each select="curr:WartoscSporu">
								<span>
									<xsl:text>Wartość przedmiotu sporu&#160;&#160; </xsl:text>
								</span>
								<span style="font-weight:bold; ">
									<xsl:apply-templates/>
								</span>
								<span style="font-weight:bold; ">
									<xsl:text> PLN</xsl:text>
								</span>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<br/>
					<div>
						<h2 style="text-align:center ">
							<span>
								<xsl:text>P O Z E W</xsl:text>
							</span>
						</h2>
					</div>
					<div align="center">
						<h3 style="text-align:center; " align="center">
							<span style="font-weight:bold; ">
								<xsl:text>o zapłatę</xsl:text>
							</span>
						</h3>
					</div>
					<br/>
					<div>
						<span>
							<xsl:text>Wnoszę o zasądzenie na rzecz </xsl:text>
						</span>
						<xsl:if test="count( curr:PozewEPU/curr:ListaPowodow/curr:Powod )&lt;=1">
							<span>
								<xsl:value-of select="&apos;Powoda&apos;"/>
							</span>
						</xsl:if>
						<xsl:if test="count( curr:PozewEPU/curr:ListaPowodow/curr:Powod )&gt;1">
							<span>
								<xsl:value-of select="&apos;Powodów&apos;"/>
							</span>
						</xsl:if>
						<span>
							<xsl:text> od </xsl:text>
						</span>
						<xsl:if test="count( curr:PozewEPU/curr:ListaPozwanych/curr:Pozwany )&lt;=1">
							<span>
								<xsl:value-of select="&apos;Pozwanego&apos;"/>
							</span>
						</xsl:if>
						<span>
							<xsl:text>&#160;</xsl:text>
						</span>
						<xsl:if test="count( curr:PozewEPU/curr:ListaPozwanych/curr:Pozwany )&gt;1">
							<span>
								<xsl:value-of select="&apos;Pozwanych&apos;"/>
							</span>
						</xsl:if>
						<span>
							<xsl:text> następujących kwot:</xsl:text>
						</span>
					</div>
					<div>
						<table border="1">
							<xsl:variable name="altova:CurrContextGrid_0754B2F0" select="."/>
							<thead>
								<tr>
									<th>
										<span>
											<xsl:text>numer</xsl:text>
										</span>
									</th>
									<th>
										<span>
											<xsl:text>wartość</xsl:text>
										</span>
									</th>
									<th style="width:58px; ">
										<span>
											<xsl:text>waluta</xsl:text>
										</span>
									</th>
									<th>
										<span>
											<xsl:text>Data wymagalności</xsl:text>
										</span>
									</th>
									<th style="width:1px; ">
										<span>
											<xsl:text>opis</xsl:text>
										</span>
									</th>
									<th style="width:364px; ">
										<span>
											<xsl:text>odsetki</xsl:text>
										</span>
									</th>
									<th>
										<span>
											<xsl:text>dowody</xsl:text>
										</span>
									</th>
								</tr>
							</thead>
							<tbody>
								<xsl:for-each select="curr:PozewEPU">
									<xsl:for-each select="curr:ListaRoszczen">
										<xsl:for-each select="curr:Roszczenie">
											<tr>
												<td align="center">
													<xsl:for-each select="@numer">
														<span>
															<xsl:value-of select="string(.)"/>
														</span>
													</xsl:for-each>
												</td>
												<td align="right">
													<xsl:for-each select="@wartosc">
														<span>
															<xsl:value-of select="string(.)"/>
														</span>
													</xsl:for-each>
												</td>
												<td style="width:58px; " align="right">
													<xsl:for-each select="@waluta">
														<span>
															<xsl:value-of select="string(.)"/>
														</span>
													</xsl:for-each>
												</td>
												<td  style="width:120px; " align="center">
													<xsl:for-each select="@dataWymagalnosci">
														<span>
															<xsl:value-of select="string(.)"/>
														</span>
													</xsl:for-each>
												</td>
												<td style="width:1px; ">
													<xsl:if test="(@solidarnie=0 or (@solidarnie=1 and (string-length( @opis )=0 or name(@opis)=&apos;&apos;))) and (count( ../../curr:ListaPowodow/curr:Powod )+count(../../curr:ListaPozwanych/curr:Pozwany)&gt;2)">
														<span>
															<xsl:text>solidarnie</xsl:text>
														</span>
													</xsl:if>
													<xsl:if test="@solidarnie=4">
														<span>
															<xsl:text>in solidum</xsl:text>
														</span>
													</xsl:if>
													<xsl:if test="@solidarnie=2">
														<span>
															<xsl:text>solidarność bierna</xsl:text>
														</span>
													</xsl:if>
													<xsl:if test="@solidarnie=3">
														<span>
															<xsl:text>solidarność czynna</xsl:text>
														</span>
													</xsl:if>
													<br/>
													
													<xsl:for-each select="@opis">
														<span>
															<xsl:value-of select="string(.)"/>
														</span>
													</xsl:for-each>
												</td>
												<td style="width:364px; " align="center">
													<xsl:if test="@odsetki=0">
														<xsl:for-each select="@odsetki">
															<span>
																<xsl:text>bez odsetek</xsl:text>
															</span>
														</xsl:for-each>
													</xsl:if>
													<xsl:if test="@odsetki=1">
														<xsl:for-each select="@odsetki">
															<span>
																<xsl:text>z odsetkami </xsl:text>
															</span>
														</xsl:for-each>
													</xsl:if>
													<xsl:for-each select="curr:Odsetki">
														<xsl:for-each select="curr:OkresOdsetkowy[(@czyUstawowe=0 or @czyUstawowe=3) and name(@dataDo)]">
															<br/>
															<xsl:choose>
																<xsl:when test="@czyUstawowe=0">
																	<span>
																		<xsl:text>ustawowymi</xsl:text>
																	</span>
																</xsl:when>
																<xsl:when test="@czyUstawowe=3">
																	<span>
																		<xsl:text>umownymi w wysokości czterokrotności stopy lombardowej NBP</xsl:text>
																	</span>
																</xsl:when>
															</xsl:choose>
															<span>
																<xsl:text>&#160;</xsl:text>
															</span>
															<xsl:choose>
																<xsl:when test="@odWniesienia=&apos;1&apos;">
																	<span>
																		<xsl:text>od dnia wniesienia pozwu </xsl:text>
																	</span>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:for-each select="@dataOd">
																		<span>
																			<xsl:text>od </xsl:text>
																		</span>
																		<span>
																			<xsl:value-of select="string(.)"/>
																		</span>
																	</xsl:for-each>
																</xsl:otherwise>
															</xsl:choose>
															<span>
																<xsl:text>&#160;</xsl:text>
															</span>
															<xsl:for-each select="@dataDo">
																<span>
																	<xsl:text> do </xsl:text>
																</span>
																<span>
																	<xsl:value-of select="string(.)"/>
																</span>
															</xsl:for-each>
															<xsl:if test="@kwota&gt;0 and @kwota!=../../@wartosc">
																<xsl:for-each select="@kwota">
																	<span>
																		<xsl:text> od kwoty </xsl:text>
																	</span>
																	<span>
																		<xsl:value-of select="string(.)"/>
																	</span>
																</xsl:for-each>
																<span>
																	<xsl:text>&#160;</xsl:text>
																</span>
																<span>
																	<xsl:value-of select="../../@waluta"/>
																</span>
															</xsl:if>
															<span>
																<xsl:text>&#160;</xsl:text>
															</span>
														</xsl:for-each>
														<xsl:for-each select="curr:OkresOdsetkowy[(@czyUstawowe=0 or @czyUstawowe=3) and (not(name(@dataDo)) or name(@dataDo)='')]">
															<br/>
															<xsl:choose>
																<xsl:when test="@czyUstawowe=0">
																	<span>
																		<xsl:text>ustawowymi</xsl:text>
																	</span>
																</xsl:when>
																<xsl:when test="@czyUstawowe=3">
																	<span>
																		<xsl:text>umownymi w wysokości czterokrotności stopy lombardowej NBP</xsl:text>
																	</span>
																</xsl:when>
															</xsl:choose>
															<span>
																<xsl:text>&#160;</xsl:text>
															</span>
															<xsl:choose>
																<xsl:when test="@odWniesienia=&apos;1&apos;">
																	<span>
																		<xsl:text>od dnia wniesienia pozwu </xsl:text>
																	</span>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:for-each select="@dataOd">
																		<span>
																			<xsl:text> od </xsl:text>
																		</span>
																		<span>
																			<xsl:value-of select="string(.)"/>
																		</span>
																	</xsl:for-each>
																</xsl:otherwise>
															</xsl:choose>
															<span>
																<xsl:text> do dnia zapłaty</xsl:text>
															</span>
															<xsl:if test="@kwota&gt;0 and @kwota!=../../@wartosc">
																<xsl:for-each select="@kwota">
																	<span>
																		<xsl:text> od kwoty </xsl:text>
																	</span>
																	<span>
																		<xsl:value-of select="string(.)"/>
																	</span>
																</xsl:for-each>
																<span>
																	<xsl:text>&#160;</xsl:text>
																</span>
																<span>
																	<xsl:value-of select="../../@waluta"/>
																</span>
																<span>
																	<xsl:text>&#160;</xsl:text>
																</span>
															</xsl:if>
														</xsl:for-each>
														<xsl:for-each select="curr:OkresOdsetkowy[(@czyUstawowe=1 or @czyUstawowe=2) and name(@dataDo)]">
															<br/>
															<span>
																<xsl:text>umownymi </xsl:text>
															</span>
															<xsl:choose>
																<xsl:when test="@odWniesienia=&apos;1&apos;">
																	<span>
																		<xsl:text>od dnia wniesienia pozwu </xsl:text>
																	</span>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:for-each select="@dataOd">
																		<span>
																			<xsl:text>od </xsl:text>
																		</span>
																		<span>
																			<xsl:value-of select="string(.)"/>
																		</span>
																	</xsl:for-each>
																</xsl:otherwise>
															</xsl:choose>
															<xsl:for-each select="@dataDo">
																<span>
																	<xsl:text> do </xsl:text>
																</span>
																<span>
																	<xsl:value-of select="string(.)"/>
																</span>
															</xsl:for-each>
															<xsl:if test="@kwota&gt;0 and @kwota!=../../@wartosc">
																<xsl:for-each select="@kwota">
																	<span>
																		<xsl:text> od kwoty </xsl:text>
																	</span>
																	<span>
																		<xsl:value-of select="string(.)"/>
																	</span>
																</xsl:for-each>
																<span>
																	<xsl:text>&#160;</xsl:text>
																</span>
																<span>
																	<xsl:value-of select="../../@waluta"/>
																</span>
																<span>
																	<xsl:text>&#160;</xsl:text>
																</span>
															</xsl:if>
															<xsl:for-each select="@stopa">
																<span>
																	<xsl:text>&#160;</xsl:text>
																</span>
																<span>
																	<xsl:value-of select="string(.)"/>
																</span>
																<span>
																	<xsl:text> %</xsl:text>
																</span>
															</xsl:for-each>
															<span>
																<xsl:text>&#160;</xsl:text>
															</span>
															<xsl:if test="@okres=0">
																<xsl:for-each select="@okres">
																	<span>
																		<xsl:text> rocznie </xsl:text>
																	</span>
																</xsl:for-each>
															</xsl:if>
															<xsl:if test="@okres=1">
																<xsl:for-each select="@okres">
																	<span>
																		<xsl:text> kwartalnie </xsl:text>
																	</span>
																</xsl:for-each>
															</xsl:if>
															<xsl:if test="@okres=2">
																<xsl:for-each select="@okres">
																	<span>
																		<xsl:text> miesięcznie</xsl:text>
																	</span>
																</xsl:for-each>
															</xsl:if>
															<xsl:if test="@okres=3">
																<xsl:for-each select="@okres">
																	<span>
																		<xsl:text> tygodniowo</xsl:text>
																	</span>
																</xsl:for-each>
															</xsl:if>
															<xsl:if test="@okres=4">
																<xsl:for-each select="@okres">
																	<span>
																		<xsl:text> dziennie</xsl:text>
																	</span>
																</xsl:for-each>
															</xsl:if>
															<xsl:if test="@czyUstawowe=2">
																<span>
																	<xsl:text> ale nie więcej niż czterokrotność stopy lombardowej NBP</xsl:text>
																</span>
															</xsl:if>
														</xsl:for-each>
														<xsl:for-each select="curr:OkresOdsetkowy[(@czyUstawowe=1 or @czyUstawowe=2) and (not(name(@dataDo)) or name(@dataDo)='')]">
															<br/>
															<span>
																<xsl:text>umownymi </xsl:text>
															</span>
															<xsl:choose>
																<xsl:when test="@odWniesienia=&apos;1&apos;">
																	<span>
																		<xsl:text>od dnia wniesienia pozwu </xsl:text>
																	</span>
																</xsl:when>
																<xsl:otherwise>
																	<xsl:for-each select="@dataOd">
																		<span>
																			<xsl:text> od </xsl:text>
																		</span>
																		<span>
																			<xsl:value-of select="string(.)"/>
																		</span>
																	</xsl:for-each>
																</xsl:otherwise>
															</xsl:choose>
															<span>
																<xsl:text> do dnia zapłaty </xsl:text>
															</span>
															<xsl:if test="@kwota&gt;0 and @kwota!=../../@wartosc">
																<xsl:for-each select="@kwota">
																	<span>
																		<xsl:text> od kwoty </xsl:text>
																	</span>
																	<span>
																		<xsl:value-of select="string(.)"/>
																	</span>
																</xsl:for-each>
																<span>
																	<xsl:text>&#160;</xsl:text>
																</span>
																<span>
																	<xsl:value-of select="../../@waluta"/>
																</span>
															</xsl:if>
															<span>
																<xsl:text>&#160; </xsl:text>
															</span>
															<xsl:for-each select="@stopa">
																<span>
																	<xsl:value-of select="string(.)"/>
																</span>
																<span>
																	<xsl:text> %</xsl:text>
																</span>
															</xsl:for-each>
															<xsl:if test="@okres=0">
																<xsl:for-each select="@okres">
																	<span>
																		<xsl:text> rocznie </xsl:text>
																	</span>
																</xsl:for-each>
															</xsl:if>
															<xsl:if test="@okres=1">
																<xsl:for-each select="@okres">
																	<span>
																		<xsl:text> kwartalnie </xsl:text>
																	</span>
																</xsl:for-each>
															</xsl:if>
															<xsl:if test="@okres=2">
																<xsl:for-each select="@okres">
																	<span>
																		<xsl:text> miesięcznie</xsl:text>
																	</span>
																</xsl:for-each>
															</xsl:if>
															<xsl:if test="@okres=3">
																<xsl:for-each select="@okres">
																	<span>
																		<xsl:text> tygodniowo</xsl:text>
																	</span>
																</xsl:for-each>
															</xsl:if>
															<xsl:if test="@okres=4">
																<xsl:for-each select="@okres">
																	<span>
																		<xsl:text> dziennie</xsl:text>
																	</span>
																</xsl:for-each>
															</xsl:if>
															<span>
																<xsl:text>&#160;</xsl:text>
															</span>
															<xsl:if test="@czyUstawowe=2">
																<span>
																	<xsl:text> ale nie więcej niż czterokrotność stopy lombardowej NBP </xsl:text>
																</span>
															</xsl:if>
														</xsl:for-each>
													</xsl:for-each>
												</td>
												<td>
													<xsl:for-each select="curr:Dowody">
														<xsl:for-each select="curr:Dowod">
															<p>
																<span>
																	<xsl:text>dowód nr </xsl:text>
																</span>
																<xsl:apply-templates/>
															</p>
														</xsl:for-each>
													</xsl:for-each>
												</td>
											</tr>
										</xsl:for-each>
									</xsl:for-each>
								</xsl:for-each>
							</tbody>
						</table>
					</div>
					<div>
						<xsl:for-each select="curr:PozewEPU">
							<xsl:for-each select="curr:InneRoszczenia">
								<xsl:apply-templates/>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<xsl:if test="curr:PozewEPU/curr:OplataSadowa/@zasadzenie=1 or curr:PozewEPU/curr:KosztyZastepstwa/@zasadzenie=1 or curr:PozewEPU/curr:InneKoszty/@zasadzenie=1">
						<div>
							<span>
								<xsl:text>oraz</xsl:text>
							</span>
						</div>
					</xsl:if>
					<div>
						<xsl:for-each select="curr:PozewEPU">
							<xsl:for-each select="curr:OplataSadowa[@zasadzenie=1]">
								<xsl:for-each select="@wartosc">
									<span>
										<xsl:text>zasądzenie zwrotu kosztów sądowych </xsl:text>
									</span>
									<span>
										<xsl:value-of select="string(.)"/>
									</span>
									<span>
										<xsl:text> zł</xsl:text>
									</span>
								</xsl:for-each>
							</xsl:for-each>
							<xsl:for-each select="curr:KosztyZastepstwa[@zasadzenie=1 and @wartosc>0]">
								<br/>
								<xsl:for-each select="@wartosc">
									<span>
										<xsl:text>zasądzenie zwrotu kosztów zastępstwa procesowego </xsl:text>
									</span>
									<span>
										<xsl:value-of select="string(.)"/>
									</span>
									<span>
										<xsl:text> zł</xsl:text>
									</span>
								</xsl:for-each>
							</xsl:for-each>
							<xsl:for-each select="curr:KosztyZastepstwa[@zasadzenie=1 and @wgNorm=1 and @wartosc=0]">
								<br/>
								<span>
									<xsl:text>zasądzenie zwrotu kosztów zastępstwa procesowego wg norm przepisanych</xsl:text>
								</span>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<div>
						<xsl:for-each select="curr:PozewEPU">
							<xsl:for-each select="curr:InneKoszty[@zasadzenie=1]">
								<xsl:for-each select="@wartosc">
									<span>
										<xsl:text>zasądzenie zwrotu innych kosztów </xsl:text>
									</span>
									<span>
										<xsl:value-of select="string(.)"/>
									</span>
									<span>
										<xsl:text> zł</xsl:text>
									</span>
								</xsl:for-each>
								<xsl:for-each select="@opis">
									<span>
										<xsl:text> (</xsl:text>
									</span>
									<span>
										<xsl:value-of select="string(.)"/>
									</span>
									<span>
										<xsl:text>)</xsl:text>
									</span>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<xsl:if test="curr:PozewEPU/curr:OplataSadowa/@zwolnienie&gt;=1">
						<div>
							<xsl:for-each select="curr:PozewEPU">
								<xsl:for-each select="curr:PodstawaZwolnienia">
									<span>
										<xsl:text>Pozew zwolniony z kosztów sądowych </xsl:text>
									</span>
									<xsl:for-each select="@typ">
										<span>
											<xsl:text> na podstawie&#160; </xsl:text>
										</span>
										<span>
											<xsl:value-of select="string(.)"/>
										</span>
									</xsl:for-each>
									<xsl:for-each select="@opis">
										<span>
											<xsl:text>&#160;</xsl:text>
										</span>
										<span>
											<xsl:value-of select="string(.)"/>
										</span>
									</xsl:for-each>
								</xsl:for-each>
							</xsl:for-each>
						</div>
					</xsl:if>
					<div align="center">
						<span style="font-weight:bold; ">
							<xsl:text>U Z A S A D N I E N I E</xsl:text>
						</span>
					</div>
					<pre>
						<div align="left">
							<xsl:for-each select="curr:PozewEPU">
								<xsl:for-each select="curr:Uzasadnienie">
									<span style="word-wrap:break-word;font-family:Times New Roman;font-size:medium; ">
										<xsl:apply-templates/>
									</span>
								</xsl:for-each>
							</xsl:for-each>
						</div>
					</pre>
					<div>
						<xsl:for-each select="curr:PozewEPU">
							<xsl:for-each select="curr:ListaDowodow">
								<br/>
								<xsl:if test="count( curr:Dowod )&gt;0">
									<span style="text-decoration:underline; ">
										<xsl:text>Lista dowodów </xsl:text>
									</span>
								</xsl:if>
								<xsl:for-each select="curr:Dowod">
									<br/>
									<xsl:for-each select="@numer">
										<span>
											<xsl:value-of select="string(.)"/>
										</span>
										<span>
											<xsl:text>. </xsl:text>
										</span>
									</xsl:for-each>
									<xsl:for-each select="@typDowodu">
										<span>
											<xsl:value-of select="string(.)"/>
										</span>
									</xsl:for-each>
									<xsl:for-each select="@oznaczenie">
										<span>
											<xsl:text>&#160;</xsl:text>
										</span>
										<span>
											<xsl:value-of select="string(.)"/>
										</span>
									</xsl:for-each>
									<xsl:for-each select="@dataDowodu">
										<span>
											<xsl:text> z dnia </xsl:text>
										</span>
										<span>
											<xsl:value-of select="string(.)"/>
										</span>
									</xsl:for-each>
									<xsl:for-each select="curr:FaktStwierdzany">
										<br/>
										<xsl:apply-templates/>
									</xsl:for-each>
									<xsl:for-each select="curr:Opis">
										<br/>
										<xsl:apply-templates/>
									</xsl:for-each>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<br/>
					<xsl:if test="count( curr:PozewEPU/curr:ListaPodpisow/curr:Podpis )&gt;0">
						<div>
							<span style="text-decoration:underline; ">
								<xsl:text>Lista podpisów</xsl:text>
							</span>
						</div>
					</xsl:if>
					<div>
						<xsl:for-each select="curr:PozewEPU">
							<xsl:for-each select="curr:ListaPodpisow">
								<xsl:for-each select="curr:Podpis">
									<xsl:for-each select="curr:Imie">
										<br/>
										<xsl:apply-templates/>
									</xsl:for-each>
									<xsl:for-each select="curr:Imie2">
										<span>
											<xsl:text>&#160;</xsl:text>
										</span>
										<xsl:apply-templates/>
									</xsl:for-each>
									<xsl:for-each select="curr:Nazwisko">
										<span>
											<xsl:text>&#160;</xsl:text>
										</span>
										<xsl:apply-templates/>
									</xsl:for-each>
									<xsl:for-each select="curr:PESEL">
										<span>
											<xsl:text>&#160; PESEL </xsl:text>
										</span>
										<xsl:apply-templates/>
									</xsl:for-each>
									<xsl:for-each select="curr:stanowisko">
										<span>
											<xsl:text>&#160;</xsl:text>
										</span>
										<xsl:apply-templates/>
									</xsl:for-each>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<br/>
				</xsl:for-each>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
