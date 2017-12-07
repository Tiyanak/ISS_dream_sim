package hr.fer.iss.model;

import java.util.HashMap;
import java.util.Map;

/**
 * Created by Igor Farszky on 7.12.2017..
 */
public enum Type {

    CONTROL("control"),
    REWARD("reward"),
    PUNISHMENT("punishment");

    private String type;

    private static class Const{
        final static Map<Integer, Type> typeMap = new HashMap<Integer, Type>();
    }

    Type(String type) {
        this.type = type;
    }

    public String getType() {
        return type;
    }

    public static Type valueOf(int id){
        return Const.typeMap.get(id);
    }


}
