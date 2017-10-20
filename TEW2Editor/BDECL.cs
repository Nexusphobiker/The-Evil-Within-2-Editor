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
            //first encounter with mpresource byte 01 means skip resource => 0 means resource exists
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            ret = ret + "casting" + '\n';
            ret = ret + "{" + '\n';

            //lock_sound
            int doesResourceExist = streamMirror.ReadByte();
            if (doesResourceExist == 0)
            {
                ret = ret + '\t' + "lock_sound:" + '\n';
                ret = ret + '\t' + "type:"+ ReadString() + '\n';
                ret = ret + '\t' + "val:" + ReadString() + '\n';
            }
            //unlock_sound
            doesResourceExist = streamMirror.ReadByte();
            if (doesResourceExist == 0) {
                ret = ret + '\t' + "unlock_sound:" + '\n';
                ret = ret + '\t' + "type:" + ReadString() + '\n';
                ret = ret + '\t' + "val:" + ReadString() + '\n';
            }
            //rattle_sound
            doesResourceExist = streamMirror.ReadByte();
            if (doesResourceExist == 0) {
                ret = ret + '\t' + "rattle_sound:" + '\n';
                ret = ret + '\t' + "type:" + ReadString() + '\n';
                ret = ret + '\t' + "val:" + ReadString() + '\n';
            }
            //opening_sound
            doesResourceExist = streamMirror.ReadByte();
            if (doesResourceExist == 0) {
                ret = ret + '\t' + "opening_sound:" + '\n';
                ret = ret + '\t' + "type:" + ReadString() + '\n';
                ret = ret + '\t' + "val:" + ReadString() + '\n';
            }
            //kick_open_sound
            doesResourceExist = streamMirror.ReadByte();
            if (doesResourceExist == 0) {
                ret = ret + '\t' + "kick_open_sound:" + '\n';
                ret = ret + '\t' + "type:" + ReadString() + '\n';
                ret = ret + '\t' + "val:" + ReadString() + '\n';
            }
            //closing_sound
            doesResourceExist = streamMirror.ReadByte();
            if (doesResourceExist == 0) {
                ret = ret + '\t' + "closing_sound:" + '\n';
                ret = ret + '\t' + "type:" + ReadString() + '\n';
                ret = ret + '\t' + "val:" + ReadString() + '\n';
            }
            //closed_sound
            doesResourceExist = streamMirror.ReadByte();
            if (doesResourceExist == 0) {
                ret = ret + '\t' + "closed_sound:" + '\n';
                ret = ret + '\t' + "type:" + ReadString() + '\n';
                ret = ret + '\t' + "val:" + ReadString() + '\n';
            }
            //half_pushed_sound
            doesResourceExist = streamMirror.ReadByte();
            if (doesResourceExist == 0) {
                ret = ret + '\t' + "half_pushed_sound:" + '\n';
                ret = ret + '\t' + "type:" + ReadString() + '\n';
                ret = ret + '\t' + "val:" + ReadString() + '\n';
            }
            //half_kicked_sound
            doesResourceExist = streamMirror.ReadByte();
            if (doesResourceExist == 0) {
                ret = ret + '\t' + "half_kicked_sound:" + '\n';
                ret = ret + '\t' + "type:" + ReadString() + '\n';
                ret = ret + '\t' + "val:" + ReadString() + '\n';
            }
            //bang_sound
            doesResourceExist = streamMirror.ReadByte();
            if (doesResourceExist == 0) {
                ret = ret + '\t' + "bang_sound:" + '\n';
                ret = ret + '\t' + "type:" + ReadString() + '\n';
                ret = ret + '\t' + "val:" + ReadString() + '\n';
            }
            //burst_sound
            doesResourceExist = streamMirror.ReadByte();
            if (doesResourceExist == 0) {
                ret = ret + '\t' + "burst_sound:" + '\n';
                ret = ret + '\t' + "type:" + ReadString() + '\n';
                ret = ret + '\t' + "val:" + ReadString() + '\n';
            }
            ret = ret + "}" + '\n';
            return ret;
        }//implemented
        private string enemyBody()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            ret = ret + "enemyBody" + '\n';
            ret = ret + "{" + '\n';
            streamMirror.ReadByte();
            int partListLen = ReadWord();
            ret = ret + '\t' + "parts (list[" + partListLen + "])" + '\n';
            ret = ret + '\t' + "{" + '\n';
            int i = 0;
            while(i < partListLen)
            {
                ret = ret + '\t' + '\t' + "part["+i+"]" + '\n';
                ret = ret + '\t' + '\t' + streamMirror.ReadByte() + '\n';
                ret = ret + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + ReadInt() + '\n' + '\n';
                i++;
            }
            ret = ret + '\t' + "}" + '\n';
            ret = ret + "}" + '\n';
            return ret;
        }//implemented

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
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            ret = ret + "equipment" + '\n';
            ret = ret + "{" + '\n';
            ret = ret + '\t' + "axe:" + ReadString() + " " +  ReadString() + '\n';
            ret = ret + '\t' + "pipe:" + ReadString() + " " + ReadString() + '\n';
            ret = ret + '\t' + "molotov:" + ReadString() + " " + ReadString() + '\n';
            ret = ret + '\t' + "lava:" + ReadString() + " " + ReadString() + '\n';
            ret = ret + "}" + '\n';
            return ret;
        }//implemented
        private string goreSetting()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            ret = ret + "goreSetting" + '\n';
            ret = ret + "{" + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + "}" + '\n';
            return ret;
        }//implemented
        private string health()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            ret = ret + "health" + '\n';
            ret = ret + "{" + '\n';
            ret = ret + '\t' + ReadString() + '\n';
            int resourceExists = streamMirror.ReadByte();

            if(resourceExists == 0) {
                ret = ret + '\t' + "handsModelMD6 type:" + ReadString() + " "+ ReadString() + '\n';
            }
            resourceExists = streamMirror.ReadByte();
            if(resourceExists == 0) {
                ret = ret + '\t' + "thirdPersonMD6 type:" + ReadString() + " " + ReadString() + '\n';
            }
            ret = ret + '\t' + ReadString() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';

            //childItem (probably wrong. no file i found contained this subclass)
            ret = ret + '\t' + '\t' + "childItem:" + '\n';
            streamMirror.ReadByte();
            int childItemListLen = ReadInt();
            int i = 0;
            while (i < childItemListLen)
            {
                resourceExists = streamMirror.ReadByte();
                if(resourceExists == 0)
                {
                    ret = ret + '\t' + '\t' + ReadString() + '\n';
                }
                i++;
            }
            //attachmentInfo (same issue as above)
            ret = ret + '\t' + '\t' + "attachmentInfo:" + '\n';
            streamMirror.ReadByte();
            int attachmentInfoListLen = ReadInt();
            i = 0;
            while (i < attachmentInfoListLen)
            {
                ret = ret + '\t' + '\t' + ReadInt() + '\n';
                ret = ret + '\t' + '\t' + ReadString() + '\n';
                i++;
            }
            ret = ret + '\t' + ReadString() + '\n';

            resourceExists = streamMirror.ReadByte();
            if (resourceExists == 0)
            {
                ret = ret + '\t' + "entityDef type:" + ReadString() + " " + ReadString() + '\n';
            }
            resourceExists = streamMirror.ReadByte();
            if (resourceExists == 0)
            {
                ret = ret + '\t' + "entityDef2 type:" + ReadString() + " " + ReadString() + '\n';
            }

            //uiInfo
            ret = ret + '\t' + '\t' + "uiInfo:" + '\n';
            ret = ret + '\t' + '\t' + ReadString()  + '\n';
            ret = ret + '\t' + '\t' + ReadString() + '\n';
            ret = ret + '\t' + '\t' + "nameStrId:" + ReadInt() + '\n';
            ret = ret + '\t' + '\t' + "descStrId:" + ReadInt() + '\n';
            ret = ret + '\t' + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + '\t' + "nameStrId_plural:" + ReadInt() + '\n';

            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';

            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';

            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';

            //droppedControllerShake
            ret = ret + '\t' + '\t' + "droppedControllerShake:" + '\n';
            ret = ret + '\t' + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + '\t' + ReadInt() + '\n';

            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';

            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';

            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';

            //useSound
            resourceExists = streamMirror.ReadByte();
            if(resourceExists == 0)
            {
                ret = ret + '\t' + "useSound type:" + ReadString() + " " + ReadString() + '\n';
            }
            //equipSound
            resourceExists = streamMirror.ReadByte();
            if (resourceExists == 0)
            {
                ret = ret + '\t' + "equipSound type:" + ReadString() + " " + ReadString() + '\n';
            }
            //unequipSound
            resourceExists = streamMirror.ReadByte();
            if (resourceExists == 0)
            {
                ret = ret + '\t' + "unequipSound type:" + ReadString() + " " + ReadString() + '\n';
            }
            //breakSound
            resourceExists = streamMirror.ReadByte();
            if (resourceExists == 0)
            {
                ret = ret + '\t' + "breakSound type:" + ReadString() + " " + ReadString() + '\n';
            }

            ret = ret + '\t' + ReadInt() + '\n';

            //guiCustomMaterial
            resourceExists = streamMirror.ReadByte();
            if (resourceExists == 0)
            {
                ret = ret + '\t' + "guiCustomMaterial type:" + ReadString() + " " + ReadString() + '\n';
            }

            ret = ret + '\t' + '\t' + "alreadyGot:" + ReadInt() + '\n';
            ret = ret + '\t' + '\t' + "craftFirstConfirmed:" + ReadInt() + '\n';
            ret = ret + '\t' + '\t' + "menuFirstConfirmed:" + ReadInt() + '\n';
            ret = ret + '\t' + '\t' + "filmConfirmed:" + ReadInt() + '\n';

            ret = ret + '\t' + streamMirror.ReadByte() + '\n';

            //showPopup
            resourceExists = streamMirror.ReadByte();
            if (resourceExists == 0)
            {
                ret = ret + '\t' + "showPopup type:" + ReadString() + " " + ReadString() + '\n';
            }

            //giveItemsOnRecieve
            streamMirror.ReadByte();
            int giveItemsOnRecieveListLength = ReadInt();
            ret = ret + '\t' + '\t' + "giveItemOnRecieve list[" + giveItemsOnRecieveListLength + "]" + '\n';
            i = 0;
            while(i < giveItemsOnRecieveListLength)
            {
                ret = ret + '\t' + '\t' + "giveItemOnRecieve type:" + ReadString() + " " + ReadString() + '\n';
                i++;
            }

            //declFX
            resourceExists = streamMirror.ReadByte();
            if (resourceExists == 0)
            {
                ret = ret + '\t' + "declFX type:" + ReadString() + " " + ReadString() + '\n';
            }

            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + streamMirror.ReadByte() + '\n';

            ret = ret + '\t' + streamMirror.ReadByte() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';
            ret = ret + '\t' + ReadInt() + '\n';

            ret = ret + "}" + '\n';
            return ret;
        }//implemented
        private string inventoryItem()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string layer()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }//counting as implemented because no content.
        private string LoadingAreaLayer()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string loadScreen()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            ret = ret + "loadScreen" + '\n';
            ret = ret + "{" + '\n';
            streamMirror.ReadByte();
            int imageDeclsListLen = ReadInt();
            int i = 0;
            ret = ret + '\t' + "imageDecls (list[" + imageDeclsListLen + "])" + '\n';
            ret = ret + '\t' + "{" + '\n';
            while (i < imageDeclsListLen)
            {
                //is mpresource
                if (streamMirror.ReadByte() == 0)
                {
                    ret = ret + '\t' + ReadString() + "  " + ReadString() + '\n';
                }
                i++;
            }
            ret = ret + '\t' + "}" + '\n';

            ret = ret + ReadString() + '\n';
            ret = ret + "}" + '\n';
            return ret;
        }//implemented
        private string loadScreenImage()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            ret = ret + "loadScreenImage" + '\n';
            ret = ret + "{" + '\n';
            streamMirror.ReadByte();
            int imagesListLen = ReadInt();
            ret = ret + '\t' + "images list[" + imagesListLen + ']' + '\n';
            ret = ret + '\t' + '{' + '\n';
            int i = 0;
            while(i < imagesListLen)
            {
                ret = ret + '\t' + "image["+i+"]" + '\n';
                ret = ret + '\t' + ReadString() + '\n';
                ret = ret + '\t' + ReadString() + '\n';
                ret = ret + '\t' + ReadInt() + '\n';
                i++;
            }

            ret = ret + '\t' + '}' + '\n';
            ret = ret + "}" + '\n';
            return ret;
        }//implemented
        private string mapInfo()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            ret = ret + "mapInfo" + '\n';
            ret = ret + "{" + '\n';
            ret = ret + "prettyMapName:" + ReadInt() + '\n';
            ret = ret + ReadInt() + '\n';
            ret = ret + ReadInt() + '\n';
            ret = ret + ReadString() + '\n';
            ret = ret + "declLoadScreen:" + ReadString() + '\n';
            ret = ret + "declLoadMessage:" + ReadString() + '\n';
            if(streamMirror.ReadByte() == 0)
            {
                ret = ret + "option:" + ReadString() + " " + ReadString() + '\n';
            }
            ret = ret + "}" + '\n';
            return ret;
        }//implemented
        private string mapInfoOptGame()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            ret = ret + "mapInfoOptGame" + '\n';
            ret = ret + "{" + '\n';
            ret = ret + ReadInt() + '\n';
            ret = ret + ReadString() + '\n';
            ret = ret + ReadString() + '\n';
            ret = ret + streamMirror.ReadByte() + '\n';
            ret = ret + streamMirror.ReadByte() + '\n';
            ret = ret + "}" + '\n';
            return ret;
        }//implemented
        private string mapInfoSgp()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            ret = ret + "mapInfoSgp" + '\n';
            ret = ret + "{" + '\n';
            ret = ret + ReadString() + '\n';
            streamMirror.ReadByte();
            int layersListLen = ReadInt();
            ret = ret + '\t' + "layers list[" + layersListLen + "]" + '\n';
            int i = 0;
            while(i < layersListLen)
            {
                ret = ret + '\t' + "layer[" + i + "] " + ReadString() + '\n';
                i++;
            }
            ret = ret + ReadString() + '\n';
            ret = ret + ReadInt() + '\n';
            ret = ret + "}" + '\n';
            return ret;
        }//implemented
        private string mission()
        {
            return "BINARY DECL PARSER BY NEXUSPHOBIKER FILENAME:" + filename + " TYPE:" + type + " LOGIC FOR THIS TYPE NOT IMPLEMENTED";
        }
        private string missionCndAlways()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            ret = ret + "missionCndAlways" + '\n';
            ret = ret + "{" + '\n';
            ret = ret + streamMirror.ReadByte() + '\n';
            ret = ret + "}" + '\n';
            return ret;
        }//implemented
        private string missionCndGameFlag()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            ret = ret + "missionCndGameFlag" + '\n';
            ret = ret + "{" + '\n';
            ret = ret + "gameFlag:" + ReadInt() + '\n';
            ret = ret + ReadInt() + '\n';
            ret = ret + ReadInt() + '\n';
            ret = ret + "}" + '\n';
            return ret;
        }//implemented
        private string missionReward()
        {
            string ret = "BINARY DECL PARSER BY NEXUSPHOBIKER" + '\n';
            ret = ret + "FILENAME:" + filename + " TYPE:" + type + '\n';
            ret = ret + "missionReward" + '\n';
            ret = ret + "{" + '\n';
            streamMirror.ReadByte();
            int rewardsListLen = ReadInt();
            ret = ret + '\t' + "rewards list["+rewardsListLen+"]" + '\n';
            ret = ret + '\t' + '{' + '\n';
            int i = 0;
            while(i < rewardsListLen)
            {
                if(streamMirror.ReadByte() == 0)
                {
                    ret = ret + '\t' + "reward [" + i + "]" + '\n';
                    ret = ret + '\t' + "inventory:"+ReadString() + " " + ReadString() + '\n';
                    ret = ret + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + ReadInt() + '\n';
                    ret = ret + '\t' + streamMirror.ReadByte() + '\n';
                }
                i++;
            }
            ret = ret + '\t' + '}' + '\n';
            ret = ret + "}" + '\n';
            return ret;
        }//implemented
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
