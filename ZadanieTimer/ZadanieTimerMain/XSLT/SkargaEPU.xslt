<xsl:stylesheet version="1.0" xmlns:xsl="http://www.w3.org/1999/XSL/Transform" xmlns:curr="http://www.currenda.pl/epu" xmlns:fn="http://www.w3.org/2005/xpath-functions" xmlns:link="http://www.xbrl.org/2003/linkbase" xmlns:xbrldi="http://xbrl.org/2006/xbrldi" xmlns:xbrli="http://www.xbrl.org/2003/instance" xmlns:xdt="http://www.w3.org/2005/xpath-datatypes" xmlns:xlink="http://www.w3.org/1999/xlink" xmlns:xs="http://www.w3.org/2001/XMLSchema" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:altova="http://www.altova.com" exclude-result-prefixes="curr fn link xbrldi xbrli xdt xlink xs xsd xsi">
	<xsl:output version="4.0" method="html" indent="no" encoding="UTF-8" doctype-public="-//W3C//DTD HTML 4.01 Transitional//EN" doctype-system="http://www.w3.org/TR/html4/loose.dtd"/>
	<xsl:param name="SV_OutputFormat" select="'HTML'"/>
	<xsl:variable name="XML" select="/"/>
	<xsl:decimal-format name="format1" grouping-separator=" " decimal-separator=","/>
	<xsl:template match="/">
		<html>
			<head>
				<title/>
			</head>
			<body style="margin-left=0.6in; margin-right=0.6in; margin-top=0.79in; margin-bottom=0.79in">
				<xsl:for-each select="$XML">
					<br/>
					<div>
						<xsl:for-each select="curr:SkargaEPU">
							<xsl:for-each select="curr:DataSkargi">
								<span>
									<xsl:text>Data złożenia </xsl:text>
								</span>
								<span>
									<xsl:value-of select="format-number(number(substring(string(string(string(.))), 1, 4)), '0000', 'format1')"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="format-number(number(substring(string(string(.)), 6, 2)), '00', 'format1')"/>
									<xsl:text>-</xsl:text>
									<xsl:value-of select="format-number(number(substring(string(string(.)), 9, 2)), '00', 'format1')"/>
								</span>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<br/>
					<div>
						<xsl:for-each select="curr:SkargaEPU">
							<xsl:for-each select="curr:Orzeczenie">
								<xsl:for-each select="curr:Sygnatura">
									<span>
										<xsl:text>Sygnatura akt </xsl:text>
									</span>
									<span style="font-weight:bold; ">
										<xsl:apply-templates/>
									</span>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<br/>
					<div>
						<span style="text-decoration:underline; ">
							<xsl:text>Osoba składająca</xsl:text>
						</span>
					</div>
					<div>
						<xsl:for-each select="curr:SkargaEPU">
							<xsl:for-each select="curr:OsobaSkladajaca">
								<xsl:for-each select="curr:Osoba">
									<xsl:for-each select="curr:Imie">
										<xsl:apply-templates/>
									</xsl:for-each>
									<xsl:for-each select="curr:Imie2">
										<xsl:apply-templates/>
									</xsl:for-each>
									<xsl:for-each select="curr:Nazwisko">
										<xsl:apply-templates/>
									</xsl:for-each>
									<xsl:for-each select="curr:PESEL">
										<span>
											<xsl:text> PESEL </xsl:text>
										</span>
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
						<h2 align="center">
							<span>
								<xsl:text>S K A R G A</xsl:text>
							</span>
						</h2>
					</div>
					<div>
						<span>
							<xsl:text>Dotyczy </xsl:text>
						</span>
						<xsl:for-each select="curr:SkargaEPU">
							<xsl:for-each select="curr:Orzeczenie">
								<xsl:for-each select="curr:Opis">
									<xsl:apply-templates/>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
						<span>
							<xsl:text>&#160;</xsl:text>
						</span>
						<xsl:for-each select="curr:SkargaEPU">
							<xsl:for-each select="curr:Orzeczenie">
								<xsl:for-each select="curr:DataOrzeczenia">
									<span>
										<xsl:text> z dnia </xsl:text>
									</span>
									<xsl:apply-templates/>
								</xsl:for-each>
								<xsl:for-each select="curr:Sygnatura">
									<span>
										<xsl:text> w sprawie </xsl:text>
									</span>
									<span style="font-weight:bold; ">
										<xsl:apply-templates/>
									</span>
								</xsl:for-each>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<br/>
					<div>
						<xsl:for-each select="curr:SkargaEPU">
							<xsl:for-each select="curr:Tresc">
								<xsl:apply-templates/>
							</xsl:for-each>
						</xsl:for-each>
					</div>
					<br/>
					<div>
						<xsl:if test="count( curr:SkargaEPU/curr:ListaPowodow/curr:Powod )=1">
							<span>
								<xsl:text>Powód:</xsl:text>
							</span>
						</xsl:if>
						<span>
							<xsl:text>&#160; </xsl:text>
						</span>
						<xsl:if test="count( curr:SkargaEPU/curr:ListaPowodow/curr:Powod )&gt;1">
							<span>
								<xsl:text>Powodowie:</xsl:text>
							</span>
						</xsl:if>
						<ul>
							<xsl:for-each select="curr:SkargaEPU">
								<xsl:for-each select="curr:ListaPowodow">
									<xsl:for-each select="curr:Powod">
										<li>
											<xsl:for-each select="curr:OsobaFizyczna">
												<xsl:for-each select="curr:Imie">
													<xsl:apply-templates/>
												</xsl:for-each>
												<xsl:for-each select="curr:Imie2">
													<span>
														<xsl:text>&#160;</xsl:text>
													</span>
													<xsl:apply-templates/>
												</xsl:for-each>
												<xsl:for-each select="curr:Nazwisko">
													<xsl:apply-templates/>
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
												<span>
													<xsl:text>&#160;</xsl:text>
												</span>
											</xsl:for-each>
											<xsl:for-each select="curr:Instytucja">
												<xsl:for-each select="curr:Nazwa">
													<xsl:apply-templates/>
												</xsl:for-each>
												<xsl:if test="contains( curr:Siedziba , &apos;z siedzibą&apos; )=false">
													<xsl:for-each select="curr:Siedziba">
														<span>
															<xsl:text> z siedzibą w</xsl:text>
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
										</li>
									</xsl:for-each>
								</xsl:for-each>
							</xsl:for-each>
						</ul>
					</div>
					<div>
						<xsl:if test="count( curr:SkargaEPU/curr:ListaPozwanych/curr:Pozwany )=1">
							<span>
								<xsl:text>Pozwany:</xsl:text>
							</span>
						</xsl:if>
						<span>
							<xsl:text>&#160;</xsl:text>
						</span>
						<xsl:if test="count( curr:SkargaEPU/curr:ListaPozwanych/curr:Pozwany )&gt;1">
							<span>
								<xsl:text>Pozwani:</xsl:text>
							</span>
						</xsl:if>
						<xsl:for-each select="curr:SkargaEPU">
							<br/>
							<xsl:for-each select="curr:ListaPozwanych">
								<br/>
								<ul>
									<xsl:for-each select="curr:Pozwany">
										<li>
											<xsl:for-each select="curr:OsobaFizyczna">
												<xsl:for-each select="curr:Imie">
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
													<xsl:apply-templates/>
												</xsl:for-each>
												<xsl:if test="contains( curr:Siedziba , &apos;z siedzibą&apos; )=false">
													<xsl:for-each select="curr:Siedziba">
														<span>
															<xsl:text> z siedzibą w </xsl:text>
														</span>
														<xsl:apply-templates/>
													</xsl:for-each>
												</xsl:if>
												<xsl:for-each select="curr:Siedziba">
													<span>
														<xsl:text>&#160;</xsl:text>
													</span>
													<xsl:apply-templates/>
												</xsl:for-each>
											</xsl:for-each>
										</li>
									</xsl:for-each>
								</ul>
								<br/>
							</xsl:for-each>
							<br/>
						</xsl:for-each>
					</div>
				</xsl:for-each>
			</body>
		</html>
	</xsl:template>
</xsl:stylesheet>
