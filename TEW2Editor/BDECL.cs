using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace TEW2Editor
{
    public class BDECL
    {
        public TYPE type;
        public string filename;
        public string data;
        private Stream streamMirror;

        #region CONSTRUCTOR
        public BDECL(Stream stream)
        {
            //try {
                stream.Seek(0, SeekOrigin.Begin);

                //type
                byte[] typeLenBuff = new byte[4];
                stream.Read(typeLenBuff, 0, typeLenBuff.Length);
                byte[] typeBuff = new byte[BitConverter.ToInt32(typeLenBuff, 0)];
                stream.Read(typeBuff, 0, typeBuff.Length);
                type = (TYPE)Enum.Parse(typeof(TYPE), Encoding.Default.GetString(typeBuff));

                //filename
                byte[] filenameLenBuff = new byte[4];
                stream.Read(filenameLenBuff, 0, filenameLenBuff.Length);
                byte[] filenameBuff = new byte[BitConverter.ToInt32(filenameLenBuff, 0)];
                stream.Read(filenameBuff, 0, filenameBuff.Length);
                filename = Encoding.Default.GetString(filenameBuff);

                //get according method
                streamMirror = stream;
            string test = type.ToString();
                MethodInfo bdeclMethod = this.GetType().GetMethod(type.ToString(), BindingFlags.Instance | BindingFlags.NonPublic);
                data = (string)bdeclMethod.Invoke(this, null); 
                //the caller has to close the stream
           /* }
            catch(Exception ex)
            {
                Console.WriteLine("[BDECL] Exception while parsing binary decl:" + ex.Message);
            }*/
        }
        #endregion

        #region ENUM
        public enum TYPE
        {
            //66 different types. 
            achievement,
            ammo,
            attackTable,
            casting,
            collectionItem,
            commentcall,
            craftItem,
            cutscene,
            cutsceneEv,
            cutsceneLit,
            cutsceneSnd,
            cutsceneSub,
            cutsceneVfx,
            cutsceneVib,
            damage,
            damagePartsInfo,
            devMapJump,
            doorSounds,
            enemyBody,
            enemyMixedParts,
            enlighten,
            equipment,
            goreSetting,
            health,
            inventoryItem,
            layer,
            LoadingAreaLayer,
            loadScreen,
            loadScreenImage,
            mapInfo,
            mapInfoOptGame,
            mapInfoSgp,
            mission,
            missionCndAlways,
            missionCndGameFlag,
            missionReward,
            mphonestream,
            npcMixedParts,
            paneldisp,
            perception,
            place,
            playerProps,
            playerViewPref,
            popup,
            projectileImpactEffect,
            randomEntitySpawn,
            randomMessage,
            recorder,
            scanlog,
            slideFilm,
            talk,
            talkEnv,
            talkStream,
            throwable,
            tooltip,
            tutorial,
            uiMphone,
            uiSysCommon,
            uiSysGame,
            unlock,
            unlockCostume,
            unlockInventory,
            vehicleColor,
            weapon,
            weaponHit,
            worldicon
        }
        #endregion

        #region TYPE LOGIC
        private string achievement()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            byte[] intBuff = new byte[4];
            ret = ret + "achievement" + '\n';
            ret = ret + "{"+'\n';
            streamMirror.Read(intBuff, 0, intBuff.Length);
            ret = ret + '\t' + BitConverter.ToInt32(intBuff, 0) + '\n';
            streamMirror.Read(intBuff, 0, intBuff.Length);
            ret = ret + '\t' + BitConverter.ToInt32(intBuff, 0) + '\n';

            //rules
            streamMirror.ReadByte();
            streamMirror.Read(intBuff, 0, intBuff.Length);
            int listLength = BitConverter.ToInt32(intBuff, 0);
            ret = ret + '\t'+"rules (list["+listLength+"])" + '\n';
            ret = ret + '\t' + "{" + '\n';
            int i = 0;
            while(i < listLength)
            {
                ret = ret + '\t' + '\t' + "rule:" + '\n';
                streamMirror.Read(intBuff, 0, intBuff.Length);
                ret = ret + '\t' + '\t' + BitConverter.ToInt32(intBuff, 0) + '\n';
                streamMirror.Read(intBuff, 0, intBuff.Length);
                ret = ret + '\t' + '\t' + BitConverter.ToInt32(intBuff, 0) + '\n';
                streamMirror.Read(intBuff, 0, intBuff.Length);
                ret = ret + '\t' + '\t' + BitConverter.ToInt32(intBuff, 0) + '\n';
                streamMirror.Read(intBuff, 0, intBuff.Length);
                ret = ret + '\t' + '\t' + BitConverter.ToInt32(intBuff, 0) + '\n';
                i++;
            }
            ret = ret + '\t' + "}" + '\n';

            //completionStats
            streamMirror.ReadByte();
            streamMirror.Read(intBuff, 0, intBuff.Length);
            listLength = BitConverter.ToInt32(intBuff, 0);
            ret = ret + '\t' + "completionStats (list[" + listLength + "])" + '\n';
            ret = ret + '\t' + "{" + '\n';
            i = 0;
            while (i < listLength)
            {
                ret = ret + '\t' + '\t' + "completionStat:" + '\n';
                streamMirror.Read(intBuff, 0, intBuff.Length);
                ret = ret + '\t' + '\t' + BitConverter.ToInt32(intBuff, 0) + '\n';
                streamMirror.Read(intBuff, 0, intBuff.Length);
                ret = ret + '\t' + '\t' + BitConverter.ToInt32(intBuff, 0) + '\n';
                i++;
            }
            ret = ret + '\t' + "}" + '\n';

            //rewards note:most of this is assumed because there is no file which includes this in the game files
            streamMirror.ReadByte();
            streamMirror.Read(intBuff, 0, intBuff.Length);
            listLength = BitConverter.ToInt32(intBuff, 0);
            ret = ret + '\t' + "rewards (list[" + listLength + "])" + '\n';
            ret = ret + '\t' + "{" + '\n';
            i = 0;
            while (i < listLength)
            {
                ret = ret + '\t' + '\t' + "mpresource" + '\n';
                ret = ret + '\t' + '\t' + "{" + '\n';
                streamMirror.Read(intBuff, 0, intBuff.Length);
                ret = ret + '\t' + '\t' + BitConverter.ToInt32(intBuff, 0) + '\n';
                ret = ret + '\t' + '\t' + "}" + '\n';
                i++;
            }
            ret = ret + '\t' + "}" + '\n';

            ret = ret + "}" + '\n';

            return ret;
        } //implemented

        private string ammo()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            byte[] intBuff = new byte[4];
            ret = ret + "ammo" + '\n';
            ret = ret + "{" + '\n';
            return ret;
        }
        private string attackTable()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            byte[] intBuff = new byte[4];
            byte[] wordBuff = new byte[2];
            ret = ret + "attackTable" + '\n';
            ret = ret + "{" + '\n';

            //requires
            streamMirror.ReadByte();
            streamMirror.Read(wordBuff, 0, wordBuff.Length);
            int listLength = BitConverter.ToInt16(wordBuff, 0);
            ret = ret + '\t' + "requires (list[" + listLength + "])" + '\n';
            ret = ret + '\t' + "{" + '\n';
            int i = 0;
            while (i < listLength)
            {
                ret = ret + '\t' + '\t' + "requirement:" + '\n';
                ret = ret + '\t' + '\t' + '\t' + ReadString() + '\n';

                //parts
                ret = ret + '\t' + '\t' + '\t' + "parts:" + '\n';
                streamMirror.ReadByte();
                streamMirror.Read(wordBuff, 0, wordBuff.Length);
                int partListLen = BitConverter.ToInt16(wordBuff,0);
                int ii = 0;
                while(ii < partListLen)
                {
                    ret = ret + '\t' + '\t' + '\t' + '\t' + "part:" + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + streamMirror.ReadByte() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadNames() + '\n';
                    ii++;
                }

                //weapons
                ret = ret + '\t' + '\t' + '\t' + "weapons:" + '\n';
                streamMirror.ReadByte();
                streamMirror.Read(wordBuff, 0, wordBuff.Length);
                int weaponListLen = BitConverter.ToInt16(wordBuff, 0);
                ii = 0;
                while (ii < weaponListLen)
                {
                    ret = ret + '\t' + '\t' + '\t' + '\t' + "weapons:" + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + streamMirror.ReadByte() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + streamMirror.ReadByte() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadNames() + '\n';
                    ii++;
                }

                i++;
            }
            ret = ret + '\t' + "}" + '\n';
            
            //elements
            streamMirror.ReadByte();
            streamMirror.Read(wordBuff, 0, wordBuff.Length);
            listLength = BitConverter.ToInt16(wordBuff, 0);
            ret = ret + '\t' + "elements (list[" + listLength + "])" + '\n';
            ret = ret + '\t' + "{" + '\n';
            i = 0;
            while (i < listLength)
            {
                Console.WriteLine(i + "/" + listLength);
                ret = ret + '\t' + '\t' + "element:" + '\n';
                ret = ret + '\t' + '\t' + '\t' + ReadByteArray(15) + '\n';
                ret = ret + '\t' + '\t' + '\t' + ReadString() + '\n';
                ret = ret + '\t' + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + '\t' + ReadString() + '\n';
                ret = ret + '\t' + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + '\t' + ReadInt() + '\n';

                //targetArcs
                streamMirror.ReadByte();
                streamMirror.Read(wordBuff, 0, wordBuff.Length);
                int targetArcsLength = BitConverter.ToInt16(wordBuff, 0);
                int ii = 0;
                while (ii < targetArcsLength)
                {
                    ret = ret + '\t' + '\t' + '\t' + '\t' + "targetArcs:" + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ii++;
                }

                //arcs
                streamMirror.ReadByte();
                streamMirror.Read(wordBuff, 0, wordBuff.Length);
                int arcsLength = BitConverter.ToInt16(wordBuff, 0);
                ii = 0;
                while (ii < arcsLength)
                {
                    ret = ret + '\t' + '\t' + '\t' + '\t' + "arcs:" + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ii++;
                }

                //watchInfos
                streamMirror.ReadByte();
                streamMirror.Read(wordBuff, 0, wordBuff.Length);
                int watchInfosLength = BitConverter.ToInt16(wordBuff, 0);
                ii = 0;
                while (ii < watchInfosLength)
                {
                    ret = ret + '\t' + '\t' + '\t' + '\t' + "watchInfos:" + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + streamMirror.ReadByte() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                    ii++;
                }

                ret = ret + '\t' + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + '\t' + ReadInt() + '\n';

                ret = ret + '\t' + '\t' + '\t' + '\t' + "movementScale:" + '\n';
                ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + '\t' + '\t' + ReadInt() + '\n';

                ret = ret + '\t' + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + '\t' + ReadInt() + '\n';

                ret = ret + '\t' + '\t' + '\t' + ReadNames() + '\n';

                i++;
            }
            ret = ret + '\t' + "}" + '\n';
            ret = ret + "}" + '\n';
            return ret;
        }//implemented
        private string casting()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            ret = ret + "casting" + '\n';
            ret = ret + "{" + '\n';
            //Skip object declaration
            streamMirror.ReadByte();
            int nameInfoListLen = ReadInt();
            ret = ret + '\t' + "nameInfoList (list[" + nameInfoListLen + "])" + '\n';
            ret = ret + '\t' + "{" + '\n';
            int i = 0;
            while(i < nameInfoListLen)
            {
                ret = ret + '\t' + '\t' + "nameInfo:" + '\n';
                ret = ret + '\t' + '\t' + ReadString() + '\n';
                ret = ret + '\t' + '\t' + "line:" + ReadInt() + '\n';
                ret = ret + '\n';
                i++;
            }
            ret = ret + '\t' + "}" + '\n';
            ret = ret + "}" + '\n';
            return ret;
        }//implemented
        private string collectionItem()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string commentcall()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string craftItem()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string cutscene()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string cutsceneEv()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string cutsceneLit()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string cutsceneSnd()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string cutsceneSub()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string cutsceneVfx()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string cutsceneVib()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string damage()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string damagePartsInfo()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string devMapJump()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string doorSounds()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string enemyBody()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }

        private string enemyMixedParts()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string enlighten()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string equipment()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string goreSetting()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string health()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string inventoryItem()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string layer()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string LoadingAreaLayer()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string loadScreen()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string loadScreenImage()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string mapInfo()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string mapInfoOptGame()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string mapInfoSgp()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string mission()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string missionCndAlways()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string missionCndGameFlag()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string missionReward()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string mphonestream()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string npcMixedParts()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string paneldisp()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string perception()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string place()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string playerProps()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string playerViewPref()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string popup()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string projectileImpactEffect()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string randomEntitySpawn()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string randomMessage()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            ret = ret + "idDeclRandomMessage" + '\n';
            ret = ret + "{" + '\n';
            
            streamMirror.ReadByte();
            int messagesHighListLen = ReadInt();
            ret = ret + '\t' + "messagesHigh (list[" + messagesHighListLen + "])" + '\n';
            ret = ret + '\t' + "{" + '\n';
            int i = 0;
            while (i < messagesHighListLen)
            {
                ret = ret + '\t' + '\t' + ReadInt() + '\n';
                i++;
            }
            ret = ret + '\t' + "}" + '\n';

            streamMirror.ReadByte();
            int messagesMiddleListLen = ReadInt();
            ret = ret + '\t' + "messagesMiddle (list[" + messagesMiddleListLen + "])" + '\n';
            ret = ret + '\t' + "{" + '\n';
            i = 0;
            while (i < messagesMiddleListLen)
            {
                ret = ret + '\t' + '\t' + ReadInt() + '\n';
                i++;
            }
            ret = ret + '\t' + "}" + '\n';

            streamMirror.ReadByte();
            int messagesLowListLen = ReadInt();
            ret = ret + '\t' + "messagesMiddle (list[" + messagesLowListLen + "])" + '\n';
            ret = ret + '\t' + "{" + '\n';
            i = 0;
            while (i < messagesLowListLen)
            {
                ret = ret + '\t' + '\t' + ReadInt() + '\n';
                i++;
            }
            ret = ret + '\t' + "}" + '\n';

            ret = ret + "}" + '\n';
            return ret;
        }//implemented
        private string recorder()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string scanlog()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string slideFilm()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string talk()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string talkEnv()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string talkStream()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string throwable()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string tooltip()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string tutorial()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string uiMphone()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string uiSysCommon()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string uiSysGame()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string unlock()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string unlockCostume()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string unlockInventory()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string vehicleColor()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string weapon()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string weaponHit()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string worldicon()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }

        #endregion

        #region HELPERS
        private string ReadByteArray(int len)
        {
            byte[] retDat = new byte[len];
            streamMirror.Read(retDat, 0, retDat.Length);
            string ret = "";
            foreach(byte b in retDat)
            {
                ret = ret + ' ' + b;
            }
            return ret;
        }
        private string ReadString()
        {
            byte[] intBuff = new byte[4];
            streamMirror.Read(intBuff, 0, intBuff.Length);
            byte[] charBuff = new byte[BitConverter.ToInt32(intBuff, 0)];
            streamMirror.Read(charBuff, 0, charBuff.Length);
            return Encoding.Default.GetString(charBuff);
        }

        private int ReadInt()
        {
            byte[] intBuff = new byte[4];
            streamMirror.Read(intBuff, 0, intBuff.Length);
            return BitConverter.ToInt32(intBuff,0);
        }

        private int ReadWord()
        {
            byte[] wordBuff = new byte[2];
            streamMirror.Read(wordBuff, 0, wordBuff.Length);
            return BitConverter.ToInt16(wordBuff, 0);
        }

        private string ReadNames()
        {
            streamMirror.ReadByte();
            int length = ReadWord();
            List<string> ret = new List<string>();
            int i = 0;
            while(i < length)
            {
                ret.Add(ReadString());
                i++;
            }
            string retStr = "";
            foreach(var str in ret)
            {
                if(str == ret.Last())
                {
                    retStr = retStr + str;
                }
                else
                {
                    retStr = retStr + str + ',';
                }
            }
            return retStr;
        }
        #endregion
    }
}
