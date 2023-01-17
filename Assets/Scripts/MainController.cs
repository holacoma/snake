using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainController : MonoBehaviour
{
  public GameObject player;
  public float repeatRate;
  private Transform player_transform;
  public Vector3 direction;
  public Vector3 last_position;
  public Vector3 last_direction;
  public int score = 3;

  private Vector3 first_position;
  public ArrayList last_positions = new ArrayList();
  public ArrayList tail_positions = new ArrayList();
  public ArrayList tail = new ArrayList();
  public GameObject cherry;
  public GameObject cherry_on_map = null;

  Vector3 ChangeDirection(){
    var new_position = (player_transform.position + direction);
    if(last_position == new_position)
      direction = last_direction;
    return direction;
  }
  void PlayerMove(Vector3 new_direction){
    last_position = player_transform.position;
    player_transform.position += new_direction;
    last_direction = new_direction;
  }

  void Start(){
    player_transform = player.transform;
    first_position =  player_transform.position;
    CherryTick();
    InvokeRepeating("MainThick", 0, repeatRate);
    // InvokeRepeating("CherryTick", 0, 10f);
  }

  void Restart(){
    score = 3;
    player_transform.position = first_position;

    foreach (GameObject to_destroy in tail) {
      Destroy(to_destroy);
    }
    last_positions.Clear();
    tail_positions.Clear();
    tail.Clear();
      
  }

  void CherryTick(){
    if (cherry_on_map == null){
      cherry_on_map = GameObject.Instantiate(cherry);
      cherry_on_map.SetActive(true);
    }
    
    int x = (int) Random.Range(-10.0f, 10.0f); 
    int y = (int) Random.Range(-10.0f, 10.0f);
  
    score += 1;
    cherry_on_map.transform.position = new Vector3(x+0.5f, y+0.5f, 0);
  }

  void MainThick(){
    DisplayTail();
    Vector3 new_direction = ChangeDirection();
    PlayerMove(new_direction);
    PlayerCollide();
    PlayerOutbounds();
  }

  void PlayerOutbounds(){
    if(Mathf.Abs(player_transform.position.x) >= 25.5f)
      Restart();
    if(Mathf.Abs(player_transform.position.y) >= 15.5f)
      Restart();
  }

  void PlayerCollide(){
    if (player_transform.position.Equals(cherry_on_map.transform.position))
      CherryTick();

    if (tail_positions.Contains(player_transform.position))
      Restart();
  }

  void DisplayTail(){
    if (first_position.Equals(player_transform.position))
      return;
    
    if (last_positions.Count > score){
      last_positions.RemoveAt(0);
      GameObject to_destroy =  (GameObject) tail[0];
      Destroy(to_destroy);
      tail.RemoveAt(0);
      tail_positions.RemoveAt(0);
    }
    last_positions.Add(player_transform.position);
    GameObject tail_fragment = GameObject.Instantiate(player);
    tail_fragment.transform.position = (Vector3) last_positions[last_positions.Count - 1];
    tail_positions.Add(tail_fragment.transform.position);
    tail.Add(tail_fragment);

    
    

  }

  int RawValue(float input){
    if(input > 0)
      return 1;
    else if(input < 0)
      return -1;
    else
      return 0;
  }
  // Update is called once per frame
  void Update(){
    if (Input.anyKeyDown){
      var x = RawValue(Input.GetAxis("Horizontal"));
      var y = RawValue(Input.GetAxis("Vertical"));
      if (x != 0 && y != 0)
        y = 0;

      direction = new Vector3(x, y, 0);
    }
  }
}
