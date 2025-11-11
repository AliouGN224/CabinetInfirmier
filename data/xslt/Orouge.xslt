<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
        xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
        xmlns:medical="http://www.univ-grenoble-alpes.fr/l3miage/medical"
        xmlns:act='http://www.univ-grenoble-alpes.fr/l3miage/actes'
>
    <xsl:output method="xml" indent="yes"/>
    
    <!-- Variable contenant le document acte : pour retrouver le libellé de l'acte-->
    
    <xsl:variable name="actes" select="document('../xml/actes.xml',/)/act:ngap"/>
    
    <!-- Le nom du patient reçu en paramétre -->
    
    <xsl:param name="nomPatient" select="'Orouge'"/>
    <xsl:template match="/">
        <patient>
            <xsl:apply-templates select="//medical:patients/medical:patient[medical:nom = $nomPatient]"/>
            <xsl:apply-templates select="//medical:patients/medical:patient/medical:visite[../medical:nom = $nomPatient]"/>
        </patient>
    </xsl:template>

    <!-- Template pour récupére les informations du patient -->
    
    <xsl:template match="medical:patient">
        <nom><xsl:value-of select="medical:nom"/></nom>
        <prenom><xsl:value-of select="medical:prenom"/></prenom>
        <sexe><xsl:value-of select="medical:sexe"/></sexe>
        <numero><xsl:value-of select="medical:numero"/></numero>
        <adresse>
            <rue><xsl:value-of select="medical:adresse/medical:rue"/></rue>
            <codePostal><xsl:value-of select="medical:adresse/medical:codePostal"/></codePostal>
            <ville><xsl:value-of select="medical:adresse/medical:ville"/></ville>
        </adresse>
    </xsl:template>

    <!-- Template pour récupére les visites du patient -->
    
    <xsl:template match="medical:visite">
        <xsl:variable name="date" select="@date"/>
        <xsl:variable name="idIntervenant" select="@intervenant"/>
        <visite date="{$date}">
            <xsl:apply-templates select="//medical:infirmiers/medical:infirmier[@id = $idIntervenant]"/>
            <xsl:variable name="idActe" select="medical:acte/@id"/>
            <xsl:apply-templates select="$actes/act:actes/act:acte[@id = $idActe]"/>
        </visite>
    </xsl:template>

    <!-- Template pour récupérer l'intervenant  -->
    <xsl:template match="medical:infirmier">
        <intervenant>
            <nom><xsl:value-of select="medical:nom"/></nom>
            <prenom><xsl:value-of select="medical:prenom"/></prenom>
        </intervenant>
    </xsl:template>

    <!-- Template pour récupérer le libélé le l'acte  -->
    
    <xsl:template match="act:acte">
        <acte><xsl:value-of select="text()"/></acte>
    </xsl:template>
    
</xsl:stylesheet>