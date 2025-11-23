<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet
        version="1.0"
        xmlns:xsl="http://www.w3.org/1999/XSL/Transform" 
        xmlns:inf="http://www.univ-grenoble-alpes.fr/l3miage/medical"
        xmlns:act='http://www.univ-grenoble-alpes.fr/l3miage/actes'>

    <xsl:output method="html" encoding="UTF-8" indent="yes"/>

    <xsl:param name="destinedId">001</xsl:param>
    <xsl:variable name="actes" select="document('../xml/actes.xml', /)/act:ngap"/>

    <!-- Template Racine -->
    <xsl:template match="/">
        <html>
            <head>
                <title>Page Infirmier</title>
                <link rel="stylesheet" type="text/css" href="../css/infirmier.css"/>
                <script type="text/javascript" src="../js/facture.js"/>
            </head>
            <body>
                <h1>Page Infirmiere</h1>
                <xsl:apply-templates select="inf:cabinet/inf:infirmiers/inf:infirmier">
                    <xsl:with-param name="paramIdInf" select="$destinedId"/>
                </xsl:apply-templates>
                
                <xsl:call-template name="listePatients" />
            </body>
        </html>
    </xsl:template>

    <!-- Template bienvenue -->
    <xsl:template match="inf:infirmier">
        <xsl:param name="paramIdInf"/>
        <xsl:variable name="selectedId" select="@id"/>
        <xsl:if test="$selectedId = $paramIdInf">
            <p>
                Bonjour <xsl:value-of select="inf:prenom"/> <xsl:text> </xsl:text> <xsl:value-of select="inf:nom"/>
            </p>
            <p>
                <xsl:variable name="nbPatients" select="count(../../inf:patients/inf:patient[inf:visite/@intervenant = $paramIdInf])"/>
                Aujourd'hui vous avez <xsl:value-of select="$nbPatients"/> patient(s).
            </p>
        </xsl:if>        
    </xsl:template>

    <!-- La liste des patients et des soins -->
    <xsl:template name="listePatients">
        <table>
            <thead>
                <tr>
                    <th>Prenom et Nom</th>
                    <th>Adresse</th>
                    <th>Soins</th>
                    <th>Actions</th>
                </tr>
            </thead>
            <tbody>
                <xsl:apply-templates select="inf:cabinet/inf:patients/inf:patient">
                    <xsl:with-param name="paramIdIntervenant" select="$destinedId"/>
                </xsl:apply-templates>
            </tbody>
        </table>
    </xsl:template>
    
    <xsl:template match="inf:patient">
        <xsl:param name="paramIdIntervenant"/>
        <xsl:variable name="idIntervenant" select="inf:visite/@intervenant"/>
        <xsl:if test="$idIntervenant = $paramIdIntervenant">
            <tr>
                <td> 
                    <xsl:value-of select="inf:prenom"/> <xsl:text> </xsl:text> 
                    <xsl:value-of select="inf:nom"/> 
                </td>
                <td> 
                    <xsl:value-of select="inf:adresse/inf:numero"/> <xsl:text> </xsl:text>
                    <xsl:value-of select="inf:adresse/inf:rue"/> <xsl:text>, </xsl:text>
                    <xsl:value-of select="inf:adresse/inf:codePostal"/> <xsl:text> </xsl:text>
                    <xsl:value-of select="inf:adresse/inf:ville"/>
                </td>
                <td> 
                    <xsl:variable name="idActe" select="inf:visite/inf:acte/@id"/>
                    <ul>
                        <xsl:call-template name="recupererActe" >
                            <xsl:with-param name="paramIdActe" select="$idActe"/>
                        </xsl:call-template>
                    </ul>
                </td>
                <td>
                    <xsl:call-template name="bouton">
                        <xsl:with-param name="nomPatient" select="inf:nom"/>
                        <xsl:with-param name="prenomPatient" select="inf:prenom"/>
                        <xsl:with-param name="actesPatient" select="inf:visite/inf:acte/@id"/>
                    </xsl:call-template>
                </td>
            </tr>
        </xsl:if>
    </xsl:template>
    
    <xsl:template name="recupererActe">
        <xsl:param name="paramIdActe"/>

        <xsl:for-each select="$actes/act:actes/act:acte[@id = $paramIdActe]">
            <li>
                <xsl:value-of select="text()"/>
            </li>
        </xsl:for-each>
        
    </xsl:template>
    <!-- ...... -->
    <xsl:template name="bouton">
        <xsl:param name="nomPatient"/>
        <xsl:param name="prenomPatient"/>
        <xsl:param name="actesPatient"/>
        <xsl:element name="button">
            <xsl:attribute name="onclick">
                <xsl:text>openFacture('</xsl:text>
                <xsl:value-of select="$prenomPatient"/>
                <xsl:text>','</xsl:text>
                <xsl:value-of select="$nomPatient"/>
                <xsl:text>','</xsl:text>
                <xsl:value-of select="$actesPatient"/>
                <xsl:text>')</xsl:text>
                <!--
                openFacture('<xsl:value-of select="$prenomPatient"/>',
                            '<xsl:value-of select="$nomPatient"/>',
                            <xsl:value-of select="$actesPatient"/>)
                -->
            </xsl:attribute>

            <xsl:text>Facture</xsl:text>
        </xsl:element>
    </xsl:template>



</xsl:stylesheet>