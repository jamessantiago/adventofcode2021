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
            public long version;
            public long packetType;
            public long literalValue;
            public long length;
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
                //if (packet.length % 4 != 0)
                //    packet.length += (4 - (packet.length % 4));
                packet.literalValue = sb.ToString().ToDecimal();
                return packet;
            } else
            {
                packet.length++;
                if (rawPacket[6] == '0')
                {
                    var subPacketLengths = rawPacket.Substring(7, 15).ToDecimal();
                    packet.length += subPacketLengths + 15;
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
                        subPacketPos += (int)newPacket.length;
                        packet.packets.Add(newPacket);
                    }
                } else
                {
                    var numPackets = rawPacket.Substring(7, 11).ToDecimal();
                    packet.length += 11;
                    var subPacketPos = 18;
                    while (numPackets > 0)
                    {
                        var newPacket = ParsePacket(rawPacket[subPacketPos..]);
                        packet.length += newPacket.length;
                        subPacketPos += (int)newPacket.length;
                        packet.packets.Add(newPacket);
                        numPackets--;
                    }
                }

                switch (packet.packetType)
                {
                    case 0:
                        packet.literalValue = packet.packets.Sum(d => d.literalValue);
                        break;
                    case 1:
                        packet.literalValue = packet.packets.Select(d => d.literalValue).Aggregate(1L, (a, d) => a * d);
                        break;
                    case 2:
                        packet.literalValue = packet.packets.Min(d => d.literalValue);
                        break;
                    case 3:
                        packet.literalValue = packet.packets.Max(d => d.literalValue);
                        break;
                    case 5:
                        packet.literalValue = packet.packets[0].literalValue > packet.packets[1].literalValue ? 1 : 0;
                        break;
                    case 6:
                        packet.literalValue = packet.packets[0].literalValue < packet.packets[1].literalValue ? 1 : 0;
                        break;
                    case 7:
                        packet.literalValue = packet.packets[0].literalValue == packet.packets[1].literalValue ? 1 : 0;
                        break;
                    default:
                        throw new Exception();
                }
                return packet;
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
            StringBuilder sb = new StringBuilder();
            foreach (var c in _input.ToCharArray()) sb.Append(c.HexToBinary());
            var encap = sb.ToString();

            var packet = ParsePacket(encap);

            return new(packet.literalValue.ToString());
        }
                
    }
}
