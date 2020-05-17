﻿using System.Collections;
using System.Collections.Generic;
using Station;
using UnityEngine;

namespace Station
{
    
    public class EquipmentContainer : ItemContainer
    {
        #region FIELDS
        Dictionary<int, EquipmentSlotModel> _slotMaping = new Dictionary<int, EquipmentSlotModel>();
        private EquipmentSlotsDb _equipmentSlotsDb;
        private EquipmentTypesDb _equipmentTypesDb;
        
        #endregion
        //mapping layer for each slot
        public EquipmentContainer(string id, ContainerState state, ItemsDb itemsDb) : base(id, state, itemsDb)
        {
            var dbSystem = RpgStation.GetSystemStatic<DbSystem>();
            _equipmentSlotsDb = dbSystem.GetDb<EquipmentSlotsDb>();
            _equipmentTypesDb = dbSystem.GetDb<EquipmentTypesDb>();

   
            for (int i = 0; i < _equipmentSlotsDb.Db.Count; i++)
            {
                _slotMaping.Add(i, _equipmentSlotsDb.GetEntry(i));
                GetState().Slots.Add(i, new ItemStack());
            }
        }

        public override bool ItemAllowed(int slot, BaseItemModel itemModel)
        {
            EquipmentItemModel equipment = (EquipmentItemModel) itemModel;
            if (equipment == null)
            {
                //this is not equipment
                return false;
            }

            if (_slotMaping.ContainsKey(slot) == false)
            {
                //this slot is missing
                return false;
            }
      

            //maybe
            return base.ItemAllowed(slot, itemModel);
        }

        int PreferedEquipmentSlot(object item)
        {
            //where this equipement should go
            return 0;
        }
    }
}

//id
//equipment type allowed
//equipment type use extra slot when equiped ? (2H)

