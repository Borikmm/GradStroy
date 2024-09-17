using Gradostroy;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

/// <summary>
/// Class for text blocks working. 
/// </summary>
public class Blocks_service
{

    public static Dictionary<string, Main_block> blocks;

    public static Action<GameEntity, string> AUpdateStatisticBlock;


    public Blocks_service(Dictionary<string, Main_block> blocks_)
    {
        Balance_service.AUpdate_balance += Update_balance_block;
        Day_cycle_service.ATimeChanged += Change_time_block;

        blocks = blocks_;

        AUpdateStatisticBlock += Update_statistic_block;
    }

    ~Blocks_service()
    {
        Balance_service.AUpdate_balance -= Update_balance_block;
        Day_cycle_service.ATimeChanged -= Change_time_block;
    }


    private void Change_time_block(int time)
    {
        blocks["Time_block"].link.Text = time.ToString() + " " + blocks["Time_block"].Spliter;
    }

    private void Update_balance_block(int balance)
    {
        blocks["Balance_block"].link.Text = blocks["Balance_block"].Spliter + " " + balance.ToString();
    }

    private void Update_statistic_block(GameEntity building, string name) // increment
    {
        if (building.BaseName == "House")
        {
            UpdateMiningSpeed(name);
            UpdateColBuildings(name);
        }

        else if(building.BaseName == "Tower")
        {
            UpdateColBuildings(name);
        }


        else if (name == "Killed")
        {
            UpdateZombie();
        }
        

    }

    private void UpdateZombie()
    {
        blocks["KilledZombie"].Text = (Convert.ToInt16(Blocks_service.blocks["KilledZombie"].Text) + 1).ToString();
        blocks["KilledZombie"].link.Text = blocks["KilledZombie"].Spliter + blocks["KilledZombie"].Text;
    }


    private void UpdateMiningSpeed(string name)
    {
        switch (name)
        {
            case "Destroy":
                blocks["MiningSpeed_block"].Text = (Convert.ToInt16(Blocks_service.blocks["MiningSpeed_block"].Text) - 1).ToString();
                blocks["MiningSpeed_block"].link.Text = blocks["MiningSpeed_block"].Text + blocks["MiningSpeed_block"].Spliter;
                break;
            case "build":
                blocks["MiningSpeed_block"].Text = (Convert.ToInt16(Blocks_service.blocks["MiningSpeed_block"].Text) + 1).ToString();
                blocks["MiningSpeed_block"].link.Text = blocks["MiningSpeed_block"].Text + blocks["MiningSpeed_block"].Spliter;
                break;
        }
    }

    private void UpdateColBuildings(string name)
    {
        switch (name)
        {
            case "Destroy":
                blocks["Col_buildings_block"].Text = (Convert.ToInt16(Blocks_service.blocks["Col_buildings_block"].Text) - 1).ToString();
                blocks["Col_buildings_block"].link.Text = blocks["Col_buildings_block"].Spliter + blocks["Col_buildings_block"].Text;
                break;
            case "build":
                blocks["Col_buildings_block"].Text = (Convert.ToInt16(Blocks_service.blocks["Col_buildings_block"].Text) + 1).ToString();
                blocks["Col_buildings_block"].link.Text = blocks["Col_buildings_block"].Spliter + blocks["Col_buildings_block"].Text;
                break;
        }
    }


    public void Start_setter()
    {
        foreach (Main_block setting in blocks.Values)
        {
            if (setting.reverse_spliter)
                setting.link.Text = setting.Text + " " + setting.Spliter;
            else
                setting.link.Text = setting.Spliter + " " + setting.Text;
        }
    }

}

