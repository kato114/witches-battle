export const trimAddress = (addr) => {
  return `${addr.substring(0, 6)}...${addr.substring(addr.length - 4)}`;
};

//Launchpad Contract

export const contract = {
  25: {
    mainWallet: "0x2080F3b056978e76913efdF7F15EaB5130B7647B",
    tokenAddress: "0x5Eb71485f0736d368ddC5f290ac217d2A877fCf9",
    multicallAddress: "0x1Ee38d535d541c55C9dae27B12edf090C608E6Fb",
  },
  default: {
    mainWallet: "0x2080F3b056978e76913efdF7F15EaB5130B7647B",
    tokenAddress: "0x5Eb71485f0736d368ddC5f290ac217d2A877fCf9",
    multicallAddress: "0x1Ee38d535d541c55C9dae27B12edf090C608E6Fb",
  },
};
