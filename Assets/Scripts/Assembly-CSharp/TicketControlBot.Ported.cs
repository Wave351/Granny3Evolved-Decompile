using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Diagnostics;
using System.Runtime.CompilerServices;

// Ported from Annotated-Pseudocode/TicketControlBot.c (Granny 3 Evolved, IL2CPP x64).
public partial class TicketControlBot
{
	    public TicketControlBot() { huntTimerMax = .3f; timerAlertMax = 10f; }
	    private void Start() { transform.position = idlePoint.position; agent.enabled = true; }
	    private void Update()
	    {
	        if (dead) { agent.enabled = false; alarmSFX.SetActive(false); patrolSFX.SetActive(false); return; }
	        sight1.LookAt(playerHead.position); sight2.LookAt(playerHead.position);
	        RaycastHit hit;
	        bool seen = Physics.Raycast(sight1.position, sight1.forward, out hit, Mathf.Infinity, layersRay)
	            || Physics.Raycast(sight2.position, sight2.forward, out hit, Mathf.Infinity, layersRay);
	        if (seen && hit.collider.gameObject.CompareTag("Player") && !alert && !travelBack && !gaveTicket && !hunting) hunting = true;
	        float distance = Vector3.Distance(transform.position, player.position);
	        timerAlertMax = distance <= agent.stoppingDistance || huntingTimer < 2.5f ? 6f : 9f;
	        var patrol = patrolSFX.GetComponent<AudioSource>();
	        if (!fadingPatrolSFX) { if (patrol.volume != 0) patrol.volume -= Time.deltaTime * 1.6f; }
	        else if (patrol.volume != 1) patrol.volume += Time.deltaTime * 1.1f;
	        if (hunting)
	        {
	            fadingPatrolSFX = true; agent.stoppingDistance = 3f; agent.destination = player.position;
	            if (distance <= agent.stoppingDistance)
	            {
	                if (ticketTrain.activeSelf) { gaveTicket = true; StartCoroutine(ActivateTicket()); }
	                else if (!alert) { alert = true; StartCoroutine(AlertActivate()); }
	            }
	            huntingTimer += Time.deltaTime;
	            if (agent.velocity != Vector3.zero || huntingTimer <= huntTimerMax) return;
	            hunting = false; TurnTo(player.position, 10f);
	        }
	        else if (alert)
	        {
	            fadingPatrolSFX = true; agent.stoppingDistance = 3f;
	            timerAlert += Time.deltaTime; huntingTimer = 0;
	            if (timerAlert >= 1.1f)
	            {
	                if (timerAlert >= timerAlertMax) { timerAlert = 0; alert = false; travelBack = true; }
	                var alarm = alarmSFX.GetComponent<AudioSource>();
	                if (!alarm.isPlaying)
	                {
	                    alarm.Play(); EmitNoise(); StartCoroutine(AlarmSound());
	                    if (!alarmSoundedOnce) alarmSoundedOnce = true;
	                }
	            }
	            if (agent.velocity == Vector3.zero) hunting = false;
	        }
	        else if (ticketTravel)
	        {
	            fadingPatrolSFX = true; agent.stoppingDistance = 1.5f;
	            agent.destination = activatePoint.position; huntingTimer = 0;
	            if (agent.velocity != Vector3.zero || Vector3.Distance(transform.position, activatePoint.position) > 2f) return;
	            if (!ticketStartedAction) { ticketStartedAction = true; StartCoroutine(TicketInsertionRobo()); }
	            TurnTo(actionDoorActivatorTrains.transform.position, 7f);
	        }
	        else if (travelBack)
	        {
	            agent.stoppingDistance = 0; fadingPatrolSFX = true; huntingTimer = 0; agent.destination = idlePoint.position;
	            if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance + .05f)
	            { travelBack = false; timerFadeTravel = 0; }
	        }
	        else
	        {
	            fadingPatrolSFX = false; huntingTimer = 0;
	            if (Vector3.Distance(transform.position, idlePoint.position) > .2f)
	            {
	                timerFadeTravel += Time.deltaTime;
	                if (timerFadeTravel >= timerFadeMaxTravel) { timerFadeTravel = 0; travelBack = true; }
	            }
	            else timerFadeTravel = 0;
	        }
	    }
	    private void TurnTo(Vector3 target, float speed)
	    {
	        Vector3 to = target - transform.position; to.y = 0;
	        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(to), Time.deltaTime * speed);
	    }
	    private void EmitNoise()
	    {
	        Instantiate(noisyGranny, SPNoisy.position, Quaternion.identity);
	        Instantiate(noisyGrandpa, SPNoisy.position, Quaternion.identity);
	    }
	    public void SilentButDeadly()
	    {
	        blowsStruck++;
	        if (blowsStruck == 1) smokeDeath1.SetActive(true);
	        else if (blowsStruck == 2) smokeDeath2.SetActive(true);
	        else if (blowsStruck == 3) DeathUponRobot();
	    }
	    public void DeathUponRobot()
	    {
	        smokeDeath3.SetActive(true); dead = true; travelBack = false; hunting = false; alert = false;
	        alarmSFX.SetActive(false);
	        lampObject.transform.parent = headUnitAnim.transform;
	        headUnitAnim.Play("RobotHeadBlow"); triggerNoisy.SetActive(true); smokeParticlesFlyOff.SetActive(true);
	        var body = doorActivator.GetComponent<Rigidbody>(); body.isKinematic = false;
	        doorActivator.layer = LayerMask.NameToLayer("Item");
	        doorActivator.AddComponent<ItemSeedData>().itemName = "DoorActivator";
	        doorActivator.GetComponent<OutlineCheck>().enabled = true;
	        agent.enabled = false;
	        body.AddForce(doorActivator.transform.forward * 15f, ForceMode.Impulse);
	        GetComponent<NavMeshObstacle>().enabled = true;
	    }
	    private IEnumerator ActivateTicket()
	    {
	        if (dead) yield break;
	        yield return new WaitForSeconds(1f);
	        ticketSFX.SetActive(true); ticketTrain.SetActive(false);
	        yield return new WaitForSeconds(.5f);
	        hunting = false; ticketTravel = true;
	    }
	    private IEnumerator TicketInsertionRobo()
	    {
	        if (dead) yield break;
	        yield return new WaitForSeconds(2f);
	        actionDoorActivatorTrains.requiredItem = gameObject;
	        actionDoorActivatorTrains.Interacted();
	        yield return new WaitForSeconds(.5f);
	        hunting = false; ticketTravel = false; travelBack = true;
	    }
	    private IEnumerator AlertActivate()
	    {
	        if (dead) yield break;
	        yield return new WaitForSeconds(1f);
	        alarmSFX.SetActive(true); alert = true;
	    }
	    private IEnumerator AlarmSound()
	    {
	        for (int i = 0; i < 4; i++)
	        {
	            yield return new WaitForSeconds(i == 3 ? 2f : 2.5f);
	            if (!dead && alert) EmitNoise();
	        }
	    }
}
