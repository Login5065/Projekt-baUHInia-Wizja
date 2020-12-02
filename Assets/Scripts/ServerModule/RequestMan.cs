using System;
using System.Collections.Generic;
using UnityEngine;

public static class RequestMan {

    static private List<Request> requests = new List<Request>();

    /**
     * <summary>
     * Sends a request to server given the correct <paramref name="rType"/> and <paramref name="jsonString"/>.
     * </summary>
     * <returns>
     * Id of the created request or -1 if it wasn't successful.
     * </returns>
     */
    static public int SendRequest(string jsonString, Request.RequestType rType) {
        Request newRequest = new Request(jsonString, rType);
        if (newRequest.GetId() != -1) {
            requests.Add(newRequest);
            if (Config.ModuleVerbose) Debug.Log(Messages.preRM + Messages.SendRequestOk);
        } else {
            Debug.LogWarning(Messages.preRM + Messages.SendRequestNotOk);
        }
        return newRequest.GetId();
    }

    /**
     * <summary>
     * Check if request has been answered. Will happily throw an exception
     * if given <paramref name="id"/> does not belong to any alive request.
     * </summary>
     * <exception cref="NullReferenceException"></exception>
     */
    static public bool CheckRequest(int id) {
        return requests.Find(x => x.GetId() == id).GetStatus();
    }

    /**
     * <summary>
     * Get response for request of given <paramref name="id"/>. Unlike <c>CheckRequest(int id)</c>
     * it doesn't throw an exception when given <paramref name="id"/> does not belong to any alive request.
     * Response can only be aquired once! After successful get operation request is immediately deleted.
     * </summary>
     */
    static public Response GetRequest(int id) {
        if (id == -1) {
            Debug.LogError(Messages.preRM + "id minus one is invalid you dummy!");
            return null;
        }
        Request getRequest = requests.Find(x => x.GetId() == id);
        Response aquiredResponse = null;
        if (getRequest != null) {
            if (getRequest.GetStatus()) {
                aquiredResponse = getRequest.GetResponse();
                if (Config.ModuleVerbose) Debug.Log(Messages.preRM + Messages.GetResponseOk);
                requests.Remove(getRequest);
            } else {
                Debug.LogWarning(Messages.preRM + Messages.GetResponseNotFound);
            }
        } else {
            Debug.LogWarning(Messages.preRM + Messages.GetResponseBadId);
        }
        return aquiredResponse;
    }
}
