# ECG VR-TSST v2.3

The Entertainment Computing Group VR-TSST (ECG VR-TSST) is a tool for experimental induction of acute mental stress in the laboratory. It is a VR adaptation of the original Trier Social Stress Test protocol developed by Kirschbaum, Pirke, and Hellhammer (1993). The ECG VR-TSST works with HTC Vive and Oculus Rift. 

The ECG VR-TSST has been successfully used to generate high stress levels in several studies (Liszio, Graf & Masuch, 2018; Liszio & Masuch, 2019). 

The application is available in the current version 2.3 in German and English and has a modular design. Different tasks for the subjects can be combined. Furthermore, the number of virtual testers can be varied. The ECG VR-TSST also has a logging module.

Further information about the scope and the empirical evaluation of the tool can currently be described here: https://doi.org/10.17185/duepublico/74774.

The ECG VR-TSST under is the [GNU General Public License v3.0](https://www.gnu.org/licenses/gpl-3.0.html). 

# Background
The original project traces back to the work of Dr. Stefan Liszio. All functions of the tool in version 1 and version 2 as well as the different development steps and and especially the results of the empirical evaluation of the effectiveness in stress induction are summarized here.

Liszio, S. Relaxation, Distraction, and Fun: Improving Well-being in Situations of Acute Emotional Distress with Virtual Reality (Doctoral dissertation, Dissertation, Duisburg, Essen, Universität Duisburg-Essen, 2021).

`@phdthesis{Liszio.2021,
 author = {Liszio, Stefan},
 year = {2021},
 title = {Relaxation, Distraction, and Fun: Improving Well-being in Situations of Acute Emotional Distress with Virtual Reality},
 url = {https://d-nb.info/1241044740/34},
 keywords = {Hochschulschrift},
 address = {Duisburg, Germany},
 publisher = {Entertainment Computing Group},
 school = {{Universit{\"a}t Duisburg-Essen}},
 doi = {10.17185/duepublico/74774},
 type = {Dissertation}
}`

# Cite as
The ECG VR-TSST is free to use for any non-commerical, science-related projects. The best way to support the project is to cite it as citations are a valuable measure of a scientific tool's relevance for the community.

To cite ECG VR-TSST in publications use:

`Entertainment Computing Group (2023). ECG VR-TSST (Version 2.3)[Computer software].`

And the BibTeX entry :

`@MISC{ECGVRTSST.2023,
 AUTHOR = {{Entertainment Computing Group}},
 TITLE = {{ECG VR-TSST (Version 2.3)[Computer software]}},
 YEAR = {2023},
 URL = {https://www.ecg.uni-due.de/}
}`

# New in Version 3.0
We revisioned and updated the code so that the programm now runs with OpenXR (see below).

## Switch between Oculus or SteamVR
### Option 1:
Go to: **Edit** -> **Project Settings** -> **XR Plug-in Management (OpenVR)** -> **Play Mode OpenXR Runtime**
Choose ***OculusOpen XR*** or ***SteamVR***

### Option2 
SteamVR: **SteamVR(Hamburger Menu)** -> **Settings** -> **Developer** and press the Button ***"Set SteamVR As OpenXR Runtime"***
Oculus: **Oculus (Desktop)** -> **Settings** -> **General** -> **OpenXR-Runtime** and press the Button ***"Set Oculus as active"***
