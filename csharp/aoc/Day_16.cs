using AoCHelper;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace aoc
{    
    public class Day_16 : BaseDay
    {
        private readonly string _input;

        public Day_16()
        {
            _input = File.ReadAllText(InputFilePath);
            
        }

        public class Packet
        {
            public int version;
            public int packetType;
            public int literalValue;
            public int length;
            public List<Packet> packets = new List<Packet>();
        }

        public Packet ParsePacket(string rawPacket)
        {            
            var packet = new Packet();
            packet.version = rawPacket[..3].ToDecimal();
            packet.packetType = rawPacket[3..6].ToDecimal();
            packet.length += 6;
            if (packet.packetType == 4)
            {
                var rawValue = rawPacket[6..];
                var sb = new StringBuilder();
                for (int i = 0; ; i += 5)
                {
                    packet.length += 5;
                    sb.Append(rawValue.AsSpan(i + 1, 4));
                    if (rawValue[i] == '0') break;
                }
                if (packet.length % 4 != 0)
                    packet.length += 4 - (packet.length % 4);
                packet.literalValue = sb.ToString().ToDecimal();
                return packet;
            } else
            {
                packet.length++;
                if (rawPacket[6] == '0')
                {
                    var subPacketLengths = rawPacket.Substring(7, 15).ToDecimal();
                    packet.length += subPacketLengths + 16;
                    var subPacketPos = 22;
                    while (subPacketLengths > 0)
                    {
                        if (rawPacket.Length - subPacketPos < 8)
                        {
                            packet.length += rawPacket.Length - subPacketPos;
                            break;
                        }
                        var newPacket = ParsePacket(rawPacket[subPacketPos..]);
                        subPacketLengths -= newPacket.length;
                        subPacketPos += newPacket.length - 1;
                        packet.packets.Add(newPacket);
                    }
                    return packet;
                } else
                {
                    var numPackets = rawPacket.Substring(7, 11).ToDecimal();
                    var subPacketPos = 18;
                    while (numPackets > 0)
                    {
                        var newPacket = ParsePacket(rawPacket[subPacketPos..]);
                        //packet.length += newPacket.length;
                        subPacketPos += newPacket.length - 1;
                        packet.packets.Add(newPacket);
                        numPackets--;
                    }
                    packet.length = packet.packets.Sum(d => d.length); // ?
                    return packet;
                }
            }

            throw new Exception();
        }

        public long CountVersions(Packet packet)
        {
            long sum = packet.version;
            foreach (var p in packet.packets)
            {
                sum += CountVersions(p);
            }
            return sum;
        }

        public override ValueTask<string> Solve_1()
        {
            StringBuilder sb = new StringBuilder();
            foreach (var c in _input.ToCharArray()) sb.Append(c.HexToBinary());
            var encap = sb.ToString();
            
            var packet = ParsePacket(encap);
            var packetSum = CountVersions(packet);

            return new(packetSum.ToString());
        }

        public override ValueTask<string> Solve_2()
        {
            
            return new("");
        }
                
    }
}
