<?xml version="1.0" encoding="UTF-8" ?>
<xsl:stylesheet version="1.0"
                xmlns:xsl="http://www.w3.org/1999/XSL/Transform"
                xmlns:medical="http://www.univ-grenoble-alpes.fr/l3miage/medical"
>
    <xsl:output method="html" encoding="UTF-8" indent="yes"/>
    
    <!--Template racine -->
    
    <xsl:template match="/">
        <html>
            <head>
                <META http-equiv="Content-Type" content="text/html; charset=UTF-8"/>
                <title>Page Patient</title>
                <link rel="stylesheet" type="text/css" href="../css/patient.css"/>
            </head>
            <body>
                <div>
                    <h1>Informations Personnelles</h1>
                    <div>
                        <strong>Numéro de sécurité social : </strong> <xsl:value-of select="patient/numero"/> <br/>
                        <strong>Nom : </strong> <xsl:value-of select="patient/nom"/> <br/>
                        <strong>Prénom : </strong><xsl:value-of select="patient/prenom"/> <br/>
                        <strong>Sexe : </strong> <xsl:value-of select="patient/sexe"/>  <br/>
                        <strong>Adresse compléte : </strong><xsl:if test="patient/adresse/numero"><xsl:value-of select="patient/adresse/numero"/> <xsl:text> </xsl:text> </xsl:if><xsl:value-of select="patient/adresse/rue"/> <xsl:text> </xsl:text> <xsl:value-of select="patient/adresse/codePostal"/> <xsl:if test="patient/adresse/etage"> <xsl:text> </xsl:text> ( <strong>étage :</strong> <xsl:value-of select="patient/adresse/etage"/> )</xsl:if> <br/>
                    </div>
                    <div>
                        <h1>Les informations de vos visites par ordre</h1>

                        <div>
                            <table>
                                <thead>
                                    <tr>
                                        <th>Date de la visite</th>
                                        <th>Prénom et Nom de l'infimier(e)</th>
                                        <th>Acte(s)</th>
                                    </tr>
                                </thead>
                                <tbody>
                                    <xsl:apply-templates select="patient/visite"><xsl:sort select="@date" order="ascending" data-type="text"/></xsl:apply-templates>
                                </tbody>
                            </table>
                        </div>
                    </div>
                </div>
            </body>
        </html>
    </xsl:template>
    
     <!-- Template pour récupérer les informations de la visite -->
    
    <xsl:template match="patient/visite">
            <tr>
                <td>
                    <xsl:value-of select="@date"/>
                </td>
                <td>
                    <xsl:value-of select="intervenant/nom"/> <xsl:text> </xsl:text>
                    <xsl:value-of select="intervenant/prenom"/>
                </td>
                <td>
                    <ul>
                      <xsl:apply-templates select="acte"/>
                    </ul>
                </td>
            </tr>
    </xsl:template>

    <!-- Template pour récupérer les informations des actes -->
    
    <xsl:template match="acte">
        <li><xsl:value-of select="text()"/></li>
    </xsl:template>
</xsl:stylesheet>