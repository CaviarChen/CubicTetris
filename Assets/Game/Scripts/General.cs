using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// for gneral information
public class General : MonoBehaviour {

    // structure for block data
    public struct Block {
	    public int[,,] block;
	    public int size;
    }

    public static readonly float cubeSize = 1.05f;
    // width is always 2
    public static readonly int length = 8;
    public static readonly int height = 10;

    public static readonly float timeForEachDrop = 1.5f;
    public static readonly float timeForEachMoveAni = 0.2f;
    // degree per second
    public static readonly float rotateSpeed = 720.0f;  


    // takes x in [0,1] and return y in [0,1]
    // (-cos(x*pi)+1) / 2
    // makes moving smooth
    public static float rubberBandFunction(float x) {
        return (-Mathf.Cos(x * Mathf.PI) + 1) / 2.0f;
    }

    // preset blocks
    public static Block[] generateBlockTemplate() {

        General.Block[] blocks = new General.Block[12];
        blocks[0].block = new int[2, 4, 4] {
        {
            {0, 0, 0, 0},
            {0, 1, 0, 0},
            {0, 0, 0, 0},
            {0, 0, 0, 0},

        },
        {
            {0, 0, 0, 0},
            {2, 3, 4, 0},
            {0, 0, 0, 0},
            {0, 0, 0, 0}
        }
        };
        // size decide the center of the block (for rotating)
        blocks[0].size = 3;

        blocks[1].block = new int[2, 4, 4] {
        {
            {0, 1, 0, 0},
            {2, 3, 4, 0},
            {0, 0, 0, 0},
            {0, 0, 0, 0},
        },
        {
            {0, 0, 0, 0},
            {0, 0, 0, 0},
            {0, 0, 0, 0},
            {0, 0, 0, 0},

        }
        };

        blocks[1].size = 3;

		blocks[2].block = new int[2, 4, 4] {
		{
			{0, 1, 2, 0},
			{0, 3, 0, 0},
			{0, 4, 0, 0},
			{0, 0, 0, 0},
		},
		{
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0},
		}
		};

		blocks[2].size = 4;

		blocks[3].block = new int[2, 4, 4] {
		{
			{0, 1, 2, 0},
			{0, 0, 3, 0},
			{0, 0, 4, 0},
			{0, 0, 0, 0},
		},
		{
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0},
		}
		};

		blocks[3].size = 4;

		blocks[4].block = new int[2, 4, 4] {
		{
			{0, 1, 0, 0},
			{0, 2, 0, 0},
			{0, 3, 0, 0},
			{0, 0, 0, 0},
		},
		{
			{0, 4, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0},
		}
		};

		blocks[4].size = 3;

		blocks[5].block = new int[2, 4, 4] {
		{
			{0, 1, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0},
		},
		{
			{0, 2, 0, 0},
			{0, 3, 0, 0},
			{0, 4, 0, 0},
			{0, 0, 0, 0},
		}
		};

		blocks[5].size = 3;

		blocks[6].block = new int[2, 4, 4] {
		{
			{0, 1, 0, 0},
			{0, 2, 0, 0},
			{0, 3, 0, 0},
			{0, 4, 0, 0},
		},
		{
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0},
			{0, 0, 0, 0},
		}
		};

		blocks[6].size = 4;

		blocks[7].block = new int[2, 4, 4] {
			{
				{0, 0, 0, 0},
				{0, 1, 2, 0},
				{3, 4, 0, 0},
				{0, 0, 0, 0},
			},
			{
				{0, 0, 0, 0},
				{0, 0, 0, 0},
				{0, 0, 0, 0},
				{0, 0, 0, 0},
			}
		};

		blocks[7].size = 3;

		blocks[8].block = new int[2, 4, 4] {
			{
				{0, 0, 0, 0},
				{1, 2, 0, 0},
				{0, 3, 4, 0},
				{0, 0, 0, 0},
			},
			{
				{0, 0, 0, 0},
				{0, 0, 0, 0},
				{0, 0, 0, 0},
				{0, 0, 0, 0},
			}
		};

		blocks[8].size = 3;

		blocks[9].block = new int[2, 4, 4] {
			{
				{0, 1, 0, 0},
				{0, 2, 0, 0},
				{0, 0, 0, 0},
				{0, 0, 0, 0},
			},
			{
				{0, 3, 0, 0},
				{0, 4, 0, 0},
				{0, 0, 0, 0},
				{0, 0, 0, 0},
			}
		};

		blocks[9].size = 3;

        blocks[10].block = new int[2, 4, 4] {
            {
                {1, 2, 0, 0},
                {3, 4, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            },
            {
                {0, 0, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
                {0, 0, 0, 0},
            }
        };

        blocks[10].size = 2;

		blocks[11].block = new int[2, 4, 4] {
			{
				{0, 0, 0, 0},
				{0, 1, 0, 0},
				{0, 0, 0, 0},
				{0, 0, 0, 0},
			},
			{
				{0, 0, 0, 0},
				{0, 0, 0, 0},
				{0, 0, 0, 0},
				{0, 0, 0, 0},
			}
		};

		blocks[11].size = 3;

        return blocks;
    }
}
