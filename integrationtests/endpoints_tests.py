import requests

# Documentation: https://docs.python-requests.org

# TODO: replace with test server url
# BASE_URL = "http://localhost:5072/cargohub"
# API_KEY = "57946a04-2885-4e43-821b-a40216ceda58"
BASE_URL = "http://localhost:5072/cargohub"
API_KEY = "5b3b8e0f-4a19-4e06-b458-19b33bc0ae6b"


def test_auth_get_clients():
    response = requests.get(f"{BASE_URL}/clients", headers={"API_KEY": API_KEY})
    assert response.status_code == 200

# TODO: work out individually before discussing in your team

########################################################################################
# Make integrationtests for resources clients, inventories, item_groups and item_lines.#
########################################################################################

########################################################################################
# integrationtests for clients:                                                        #
########################################################################################

    # (Checklist) Deze test onderhoud van alle endpoints de inputs/outputs:
    # ✓ GET    get_clients()
    # ✓ GET    get_client(client_id)
    # ✓ GET    get_orders_for_client(client_id)
    # ✓ POST   add_client(new_client)
    # ✓ PUT    update_client(client_id, updated_client)
    # ✓ DELETE remove_client(client_id)

def test_data_post_client():
    # To-Do: Toevoegen van meerdere clients.
    requests.delete(f"{BASE_URL}/clients/1", headers={"API_KEY": API_KEY})
    requests.delete(f"{BASE_URL}/clients/2", headers={"API_KEY": API_KEY})
    client_1 = {"id": 1, "name": "Raymond Inc", "address": "1296 Daniel Road Apt. 349", "city": "Pierceview", "zip_code": "28301", "province": "Colorado", "country": "United States", "contact_name": "Bryan Clark", "contact_phone": "242.732.3483x2573", "contact_email": "robertcharles@example.net"}
    client_2 = {"id": 2, "name": "Williams Ltd", "address": "2989 Flores Turnpike Suite 012", "city": "Lake Steve", "zip_code": "08092", "province": "Arkansas", "country": "United States", "contact_name": "Megan Hayden", "contact_phone": "8892853366", "contact_email": "qortega@example.net"}
    post_client_1 = requests.post(f"{BASE_URL}/clients", json=client_1, headers={"API_KEY": API_KEY})
    post_client_2 = requests.post(f"{BASE_URL}/clients", json=client_2, headers={"API_KEY": API_KEY})
    assert post_client_1.status_code == 200
    assert post_client_2.status_code == 200

def test_data_get_client():
    # To-Do: Uitlezen van client.
    response = requests.get(f"{BASE_URL}/clients/1", headers={"API_KEY": API_KEY})
    assert response.status_code == 200
    assert response.json().get('id') == 1

def test_data_get_clients():
    # To-Do: Uitlezen van clients.
    response = requests.get(f"{BASE_URL}/clients", headers={"API_KEY": API_KEY})
    assert response.status_code == 200
    assert response.json()[0].get('id') == 1
    assert response.json()[1].get('id') == 2

def test_data_get_orders_for_client():
    # To-Do: Uitlezen uit orders van client, vergelijk orders.
    requests.delete(f"{BASE_URL}/orders/1", headers={"API_KEY": API_KEY})

    order_1 = {
        "id": 1,
        "source_id": 38,
        "order_date": "1992-05-11T07:31:48Z",
        "request_date": "1992-05-15T07:31:48Z",
        "reference": "ORD04650",
        "reference_extra": "Schaap doorzichtig nooit lunch lamp.",
        "order_status": "Delivered",
        "notes": "Kleur vroeg stil tweede.",
        "shipping_notes": "Middel klimmen nobel na verstoppen tante.",
        "picking_notes": "Weer lui geur voorzichtig zo.",
        "warehouse_id": 37,
        "ship_to": 1,
        "bill_to": 1,
        "shipment_id": 4650,
        "total_amount": 1155.34,
        "total_discount": 345.29,
        "total_tax": 710.01,
        "total_surcharge": 29.26,
        "items": [
            { "item_id": "P005692", "amount": 19 },
            { "item_id": "P005510", "amount": 10 },
            { "item_id": "P004989", "amount": 7 },
            { "item_id": "P000834", "amount": 1 },
            { "item_id": "P005076", "amount": 8 },
            { "item_id": "P010121", "amount": 22 }
        ]
    }
    requests.post(f"{BASE_URL}/orders", json=order_1, headers={"API_KEY": API_KEY})

    get_orders_1 = requests.get(f"{BASE_URL}/clients/1/orders", headers={"API_KEY": API_KEY})
    assert get_orders_1.status_code == 200, f"Get orders failed: {get_orders_1.json()}"

    requests.delete(f"{BASE_URL}/orders/1", headers={"API_KEY": API_KEY})

def test_data_put_client():
    # To-Do: Update de client met andere data.
    client_1 = {"id": 1, "name": "Raymond Inc", "address": "1296 Daniel Road Apt. 349", "city": "Pierceview", "zip_code": "28301", "province": "Colorado", "country": "United States", "contact_name": "Clark Kent", "contact_phone": "242.732.3483x2573", "contact_email": "robertcharles@example.net"}
    update_client_1 = requests.put(f"{BASE_URL}/clients/1", json=client_1, headers={"API_KEY": API_KEY})
    assert update_client_1.status_code == 200

def test_data_delete_clients():
    # To-Do: Clients verwijderen en json leegmaken.
    delete_client_1 = requests.delete(f"{BASE_URL}/clients/1", headers={"API_KEY": API_KEY})
    delete_client_2 = requests.delete(f"{BASE_URL}/clients/2", headers={"API_KEY": API_KEY})
    assert delete_client_1.status_code == 200
    assert delete_client_2.status_code == 200

########################################################################################
# integrationtests for inventories:                                                    #
########################################################################################

    # (Checklist) Deze test onderhoud van alle endpoints de inputs/outputs:
    # ✓ GET    get_inventories()
    # ✓ GET    get_inventory(inventory_id)
    # ✓ POST   add_inventory(new_inventory)
    # ✓ PUT    update_inventory(inventory_id, updated_inventory)
    # ✓ DELETE remove_inventory(inventory_id)

def test_data_post_inventories():
    # To-Do: Toevoegen van inventories.
    inventory_1 = {"id": 1, "item_id": "P000001", "description": "Face-to-face clear-thinking complexity", "item_reference": "sjQ23408K", "locations": [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], "total_on_hand": 262, "total_expected": 0, "total_ordered": 80, "total_allocated": 41, "total_available": 141}
    inventory_2 = {"id": 2, "item_id": "P000002", "description": "Focused transitional alliance", "item_reference": "nyg48736S", "locations": [19800, 23653, 3068, 3334, 20477, 20524, 17579, 2271, 2293, 22717], "total_on_hand": 194, "total_expected": 0, "total_ordered": 139, "total_allocated": 0, "total_available": 55}
    post_inventory_1 = requests.post(f"{BASE_URL}/inventories", json=inventory_1, headers={"API_KEY": API_KEY})
    post_inventory_2 = requests.post(f"{BASE_URL}/inventories", json=inventory_2, headers={"API_KEY": API_KEY})
    assert post_inventory_1.status_code == 200
    assert post_inventory_2.status_code == 200

def test_data_get_inventory():
    # To-Do: Uitlezen van inventory.
    response = requests.get(f"{BASE_URL}/inventories/1", headers={"API_KEY": API_KEY})
    assert response.status_code == 200

def test_data_get_inventories():
    # To-Do: Uitlezen van inventories.
    response = requests.get(f"{BASE_URL}/inventories", headers={"API_KEY": API_KEY})
    assert response.status_code == 200

def test_data_put_inventory():
    # To-Do: Update de inventory met andere data.
    inventory_1 = {"id": 1, "item_id": "P000001", "description": "Washing Materials", "item_reference": "sjQ23408K", "locations": [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], "total_on_hand": 262, "total_expected": 0, "total_ordered": 80, "total_allocated": 41, "total_available": 141}
    update_inventory_1 = requests.put(f"{BASE_URL}/inventories/1", json=inventory_1, headers={"API_KEY": API_KEY})
    assert update_inventory_1.status_code == 200

def test_data_delete_inventories():
    # To-Do: Inventories verwijderen en json leegmaken.
    delete_inventory_1 = requests.delete(f"{BASE_URL}/inventories/1", headers={"API_KEY": API_KEY})
    delete_inventory_2 = requests.delete(f"{BASE_URL}/inventories/2", headers={"API_KEY": API_KEY})
    assert delete_inventory_1.status_code == 200
    assert delete_inventory_2.status_code == 200

########################################################################################
# integrationtests for item_groups:                                                    #
########################################################################################

    # (Checklist) Deze test onderhoud van alle endpoints de inputs/outputs:
    # ✓ POST   post_item_groups()
    # ✓ GET    get_item_groups()
    # ✓ GET    get_item_group(item_group_id)
    # ✓ GET    get_items_for_item_group(item_group_id)
    # ✓ PUT    update_item_group(item_group_id, updated_item_group)
    # ✓ DELETE remove_item_group(item_group_id)

def test_data_post_item_groups():
    # To-Do: Toevoegen van item_groups.
    item_group_1 = {"id": 1, "name": "Furniture", "description": ""}
    item_group_2 = {"id": 2, "name": "Stationery", "description": ""}
    post_item_group_1 = requests.post(f"{BASE_URL}/item_groups", json=item_group_1, headers={"API_KEY": API_KEY})
    post_item_group_2 = requests.post(f"{BASE_URL}/item_groups", json=item_group_2, headers={"API_KEY": API_KEY})
    assert post_item_group_1.status_code == 200
    assert post_item_group_2.status_code == 200

def test_data_get_item_group():
    # To-Do: Uitlezen van item_group.
    item_group_1 = requests.get(f"{BASE_URL}/item_groups/1", headers={"API_KEY": API_KEY})
    assert item_group_1.status_code == 200

def test_data_get_item_groups():
    # To-Do: Uitlezen van item_groups.
    item_groups = requests.get(f"{BASE_URL}/item_groups", headers={"API_KEY": API_KEY})
    assert item_groups.status_code == 200

def test_data_get_items_for_item_group():
    # To-Do: Uitlezen van items van item_group.
    item_1 = {"uid": "P000001", "code": "sjQ23408K", "description": "Face-to-face clear-thinking complexity", "short_description": "must", "upc_code": "6523540947122", "model_number": "63-OFFTq0T", "commodity_code": "oTo304", "item_line": 1, "item_group": 1, "item_type": 1, "unit_purchase_quantity": 47, "unit_order_quantity": 13, "pack_order_quantity": 11, "supplier_id": 34, "supplier_code": "SUP423", "supplier_part_number": "E-86805-uTM"}
    requests.post(f"{BASE_URL}/items", json=item_1, headers={"API_KEY": API_KEY})

    items_for_item_group = requests.get(f"{BASE_URL}/item_groups/1/items", headers={"API_KEY": API_KEY})
    requests.delete(f"{BASE_URL}/items/1", headers={"API_KEY": API_KEY})
    assert items_for_item_group.status_code == 200

def test_data_put_item_group():
    # To-Do: Update de item_group met andere data.
    item_group_1 = {"id": 1, "name": "Furniture", "description": ""}
    item_group_2 = {"id": 2, "name": "Stationery", "description": ""}
    update_item_group_1 = requests.put(f"{BASE_URL}/item_groups/1", json=item_group_1, headers={"API_KEY": API_KEY})
    update_item_group_2 = requests.put(f"{BASE_URL}/item_groups/2", json=item_group_2, headers={"API_KEY": API_KEY})
    assert update_item_group_1.status_code == 200
    assert update_item_group_2.status_code == 200

def test_data_delete_item_groups():
    # To-Do: Item_groups verwijderen en json leegmaken.
    delete_item_group_1 = requests.delete(f"{BASE_URL}/item_groups/1", headers={"API_KEY": API_KEY})
    delete_item_group_2 = requests.delete(f"{BASE_URL}/item_groups/2", headers={"API_KEY": API_KEY})
    assert delete_item_group_1.status_code == 200
    assert delete_item_group_2.status_code == 200

########################################################################################
# integrationtests for item_lines:                                                     #
########################################################################################

    # (Checklist) Deze test onderhoud van alle endpoints de inputs/outputs:
    # ✓ POST   post_item_lines()
    # ✓ GET    get_item_lines()
    # ✓ GET    get_item_line(item_line_id)
    # ✓ GET    get_items_for_item_line(item_line_id)
    # ✓ PUT    update_item_line(item_line_id, updated_item_line)
    # ✓ DELETE remove_item_line(item_line_id)

def test_data_post_item_lines():
    # To-Do: Toevoegen van item_lines.
    item_line_1 = {"id": 1, "name": "Furniture", "description": ""}
    item_line_2 = {"id": 2, "name": "Stationery", "description": ""}
    post_item_line_1 = requests.post(f"{BASE_URL}/item_lines", json=item_line_1, headers={"API_KEY": API_KEY})
    post_item_line_2 = requests.post(f"{BASE_URL}/item_lines", json=item_line_2, headers={"API_KEY": API_KEY})
    assert post_item_line_1.status_code == 200
    assert post_item_line_2.status_code == 200

def test_data_get_item_line():
    # To-Do: Uitlezen van item_line.
    item_line_1 = requests.get(f"{BASE_URL}/item_lines/1", headers={"API_KEY": API_KEY})
    assert item_line_1.status_code == 200

def test_data_get_item_lines():
    # To-Do: Uitlezen van item_lines.
    item_lines = requests.get(f"{BASE_URL}/item_lines", headers={"API_KEY": API_KEY})
    assert item_lines.status_code == 200

def test_data_get_items_for_item_line():
    # To-Do: Uitlezen van items van item_line.
    item_1 = {"uid": "P000001", "code": "sjQ23408K", "description": "Face-to-face clear-thinking complexity", "short_description": "must", "upc_code": "6523540947122", "model_number": "63-OFFTq0T", "commodity_code": "oTo304", "item_line": 1, "item_group": 1, "item_type": 1, "unit_purchase_quantity": 47, "unit_order_quantity": 13, "pack_order_quantity": 11, "supplier_id": 34, "supplier_code": "SUP423", "supplier_part_number": "E-86805-uTM"}
    requests.post(f"{BASE_URL}/items", json=item_1, headers={"API_KEY": API_KEY})

    items_for_item_line = requests.get(f"{BASE_URL}/item_lines/1/items", headers={"API_KEY": API_KEY})
    requests.delete(f"{BASE_URL}/items/1", headers={"API_KEY": API_KEY})
    assert items_for_item_line.status_code == 200

def test_data_put_item_line():
    # To-Do: Update de item_line met andere data.
    item_line_1 = {"id": 1, "name": "Furniture", "description": ""}
    item_line_2 = {"id": 2, "name": "Stationery", "description": ""}
    update_item_line_1 = requests.put(f"{BASE_URL}/item_lines/1", json=item_line_1, headers={"API_KEY": API_KEY})
    update_item_line_2 = requests.put(f"{BASE_URL}/item_lines/2", json=item_line_2, headers={"API_KEY": API_KEY})
    assert update_item_line_1.status_code == 200
    assert update_item_line_2.status_code == 200

def test_data_delete_item_lines():
    # To-Do: Item_lines verwijderen en json leegmaken.
    delete_item_line_1 = requests.delete(f"{BASE_URL}/item_lines/1", headers={"API_KEY": API_KEY})
    delete_item_line_2 = requests.delete(f"{BASE_URL}/item_lines/2", headers={"API_KEY": API_KEY})
    assert delete_item_line_1.status_code == 200
    assert delete_item_line_2.status_code == 200

########################################################################################
# integrationtests for item_types:                                                     #
########################################################################################

    # (Checklist) Deze test onderhoud van alle endpoints de inputs/outputs:
    # ✓ POST   post_item_types()
    # ✓ GET    get_item_types()
    # ✓ GET    get_item_type(item_type_id)
    # ✓ GET    get_items_for_item_type(item_type_id)
    # ✓ PUT    update_item_type(item_type_id, updated_item_type)
    # ✓ DELETE remove_item_type(item_type_id)

def test_data_post_item_types():
    # To-Do: Toevoegen van item_types.
    item_type_1 = {"id": 1, "name": "Furniture", "description": ""}
    item_type_2 = {"id": 2, "name": "Stationery", "description": ""}
    post_item_type_1 = requests.post(f"{BASE_URL}/item_types", json=item_type_1, headers={"API_KEY": API_KEY})
    post_item_type_2 = requests.post(f"{BASE_URL}/item_types", json=item_type_2, headers={"API_KEY": API_KEY})
    assert post_item_type_1.status_code == 200
    assert post_item_type_2.status_code == 200

def test_data_get_item_type():
    # To-Do: Uitlezen van item_type.
    item_type_1 = requests.get(f"{BASE_URL}/item_types/1", headers={"API_KEY": API_KEY})
    assert item_type_1.status_code == 200

def test_data_get_item_types():
    # To-Do: Uitlezen van item_types.
    item_types = requests.get(f"{BASE_URL}/item_types", headers={"API_KEY": API_KEY})
    assert item_types.status_code == 200

def test_data_get_items_for_item_type():
    # To-Do: Uitlezen van items van item_type.
    item_1 = {"uid": "P000001", "code": "sjQ23408K", "description": "Face-to-face clear-thinking complexity", "short_description": "must", "upc_code": "6523540947122", "model_number": "63-OFFTq0T", "commodity_code": "oTo304", "item_line": 1, "item_group": 1, "item_type": 1, "unit_purchase_quantity": 47, "unit_order_quantity": 13, "pack_order_quantity": 11, "supplier_id": 34, "supplier_code": "SUP423", "supplier_part_number": "E-86805-uTM"}
    requests.post(f"{BASE_URL}/items", json=item_1, headers={"API_KEY": API_KEY})

    items_for_item_type = requests.get(f"{BASE_URL}/item_types/1/items", headers={"API_KEY": API_KEY})
    requests.delete(f"{BASE_URL}/items/1", headers={"API_KEY": API_KEY})
    assert items_for_item_type.status_code == 200

def test_data_put_item_type():
    # To-Do: Update de item_type met andere data.
    item_type_1 = {"id": 1, "name": "Furniture", "description": ""}
    item_type_2 = {"id": 2, "name": "Stationery", "description": ""}
    update_item_type_1 = requests.put(f"{BASE_URL}/item_types/1", json=item_type_1, headers={"API_KEY": API_KEY})
    update_item_type_2 = requests.put(f"{BASE_URL}/item_types/2", json=item_type_2, headers={"API_KEY": API_KEY})
    assert update_item_type_1.status_code == 200
    assert update_item_type_2.status_code == 200

def test_data_delete_item_types():
    # To-Do: Item_types verwijderen en json leegmaken.
    delete_item_type_1 = requests.delete(f"{BASE_URL}/item_types/1", headers={"API_KEY": API_KEY})
    delete_item_type_2 = requests.delete(f"{BASE_URL}/item_types/2", headers={"API_KEY": API_KEY})
    assert delete_item_type_1.status_code == 200
    assert delete_item_type_2.status_code == 200

########################################################################################
# integrationtests for items:                                                          #
########################################################################################

    # (Checklist) Deze test onderhoud van alle endpoints de inputs/outputs:
    # X POST   post_items()
    # X GET    get_items()
    # X GET    get_item(item_id)
    # X GET    get_inventories_for_item(item_id)
    # X GET    get_inventory_totals_for_item(item_id)
    # X PUT    update_item(item_id, updated_item)
    # X DELETE remove_item(item_id)

def test_data_post_items():
    # To-Do: Toevoegen van items.
    requests.delete(f"{BASE_URL}/items/P000001", headers={"API_KEY": API_KEY})
    requests.delete(f"{BASE_URL}/items/P000002", headers={"API_KEY": API_KEY})

    item_1 = {"uid": "P000001", "code": "sjQ23408K", "description": "Face-to-face clear-thinking complexity", "short_description": "must", "upc_code": "6523540947122", "model_number": "63-OFFTq0T", "commodity_code": "oTo304", "item_line": 11, "item_group": 73, "item_type": 14, "unit_purchase_quantity": 47, "unit_order_quantity": 13, "pack_order_quantity": 11, "supplier_id": 34, "supplier_code": "SUP423", "supplier_part_number": "E-86805-uTM"}
    item_2 = {"uid": "P000002", "code": "nyg48736S", "description": "Focused transitional alliance", "short_description": "may", "upc_code": "9733132830047", "model_number": "ck-109684-VFb", "commodity_code": "y-20588-owy", "item_line": 69, "item_group": 85, "item_type": 39, "unit_purchase_quantity": 10, "unit_order_quantity": 15, "pack_order_quantity": 23, "supplier_id": 57, "supplier_code": "SUP312", "supplier_part_number": "j-10730-ESk"}
    post_item_1 = requests.post(f"{BASE_URL}/items", json=item_1, headers={"API_KEY": API_KEY})
    post_item_2 = requests.post(f"{BASE_URL}/items", json=item_2, headers={"API_KEY": API_KEY})
    assert post_item_1.status_code == 200
    assert post_item_2.status_code == 200

def test_data_get_item():
    # To-Do: Uitlezen van item.
    item_1 = requests.get(f"{BASE_URL}/items/P000001", headers={"API_KEY": API_KEY})
    assert item_1.status_code == 200

def test_data_get_items():
    # To-Do: Uitlezen van items.
    items = requests.get(f"{BASE_URL}/items", headers={"API_KEY": API_KEY})
    assert items.status_code == 200

def test_data_get_inventories_for_item():
    # To-Do: Uitlezen van inventories van item.
    inventory_1 = inventory_1 = {"id": 1, "item_id": "P000001", "description": "Face-to-face clear-thinking complexity", "item_reference": "sjQ23408K", "locations": [3211, 24700, 14123, 19538, 31071, 24701, 11606, 11817], "total_on_hand": 262, "total_expected": 0, "total_ordered": 80, "total_allocated": 41, "total_available": 141}
    requests.post(f"{BASE_URL}/inventories", json=inventory_1, headers={"API_KEY": API_KEY})

    inventories_for_item = requests.get(f"{BASE_URL}/items/P000001/inventory", headers={"API_KEY": API_KEY})
    requests.delete(f"{BASE_URL}/inventories/1", headers={"API_KEY": API_KEY})
    assert inventories_for_item.status_code == 200

def get_inventory_totals_for_item():
    # To-Do: Uitlezen van inventory totals van item.
    inventory_totals_for_item = requests.get(f"{BASE_URL}/items/P000001/inventory/totals", headers={"API_KEY": API_KEY})
    assert inventory_totals_for_item.status_code == 200

def test_data_put_items():
    # To-Do: Update de item met andere data.
    item_1 = {"uid": "P000001", "code": "sjQ23408K", "description": "Updated Description", "short_description": "must", "upc_code": "6523540947122", "model_number": "63-OFFTq0T", "commodity_code": "oTo304", "item_line": 11, "item_group": 73, "item_type": 14, "unit_purchase_quantity": 47, "unit_order_quantity": 13, "pack_order_quantity": 11, "supplier_id": 34, "supplier_code": "SUP423", "supplier_part_number": "E-86805-uTM"}
    item_2 = {"uid": "P000002", "code": "nyg48736S", "description": "Updated Description", "short_description": "may", "upc_code": "9733132830047", "model_number": "ck-109684-VFb", "commodity_code": "y-20588-owy", "item_line": 69, "item_group": 85, "item_type": 39, "unit_purchase_quantity": 10, "unit_order_quantity": 15, "pack_order_quantity": 23, "supplier_id": 57, "supplier_code": "SUP312", "supplier_part_number": "j-10730-ESk"}
    update_item_1 = requests.put(f"{BASE_URL}/items/P000001", json=item_1, headers={"API_KEY": API_KEY})
    update_item_2 = requests.put(f"{BASE_URL}/items/P000002", json=item_2, headers={"API_KEY": API_KEY})
    assert update_item_1.status_code == 200
    assert update_item_2.status_code == 200

def test_data_delete_items():
    # To-Do: Items verwijderen en json leegmaken.
    delete_item_1 = requests.delete(f"{BASE_URL}/items/P000001", headers={"API_KEY": API_KEY})
    delete_item_2 = requests.delete(f"{BASE_URL}/items/P000002", headers={"API_KEY": API_KEY})
    assert delete_item_1.status_code == 200
    assert delete_item_2.status_code == 200

########################################################################################
# integrationtests for locations:                                                      #
########################################################################################

    # (Checklist) Deze test onderhoud van alle endpoints de inputs/outputs:
    # X POST   post_locations()
    # X GET    get_locations()
    # X GET    get_location(location_id)
    # X PUT    update_location(location_id, updated_location)
    # X DELETE remove_location(location_id)

def test_data_post_locarions():
    # To-Do: Toevoegen van locations.
    requests.delete(f"{BASE_URL}/locations/1", headers={"API_KEY": API_KEY})
    requests.delete(f"{BASE_URL}/locations/2", headers={"API_KEY": API_KEY})

    location_1 = {"id": 1, "warehouse_id": 1, "code": "A.1.0", "name": "Row: A, Rack: 1, Shelf: 0"}
    location_2 = {"id": 2, "warehouse_id": 1, "code": "A.1.1", "name": "Row: A, Rack: 1, Shelf: 1"}
    post_location_1 = requests.post(f"{BASE_URL}/locations", json=location_1, headers={"API_KEY": API_KEY})
    post_location_2 = requests.post(f"{BASE_URL}/locations", json=location_2, headers={"API_KEY": API_KEY})
    assert post_location_1.status_code == 200
    assert post_location_2.status_code == 200

def test_data_get_location():
    # To-Do: Uitlezen van item.
    location_1 = requests.get(f"{BASE_URL}/locations/1", headers={"API_KEY": API_KEY})
    assert location_1.status_code == 200

def test_data_get_locations():
    # To-Do: Uitlezen van locations.
    locations = requests.get(f"{BASE_URL}/locations", headers={"API_KEY": API_KEY})
    assert locations.status_code == 200

def test_data_put_locations():
    # To-Do: Update de location met andere data.
    location_1 = {"id": 1, "warehouse_id": 1, "code": "A.1.0", "name": "Row: B, Rack: 2, Shelf: 1"}
    location_2 = {"id": 2, "warehouse_id": 1, "code": "A.1.1", "name": "Row: B, Rack: 2, Shelf: 2"}
    update_location_1 = requests.put(f"{BASE_URL}/locations/1", json=location_1, headers={"API_KEY": API_KEY})
    update_location_2 = requests.put(f"{BASE_URL}/locations/2", json=location_2, headers={"API_KEY": API_KEY})
    assert update_location_1.status_code == 200
    assert update_location_2.status_code == 200

def test_data_delete_locations():
    # To-Do: locations verwijderen en json leegmaken.
    delete_location_1 = requests.delete(f"{BASE_URL}/locations/1", headers={"API_KEY": API_KEY})
    delete_location_2 = requests.delete(f"{BASE_URL}/locations/2", headers={"API_KEY": API_KEY})
    assert delete_location_1.status_code == 200
    assert delete_location_2.status_code == 200

########################################################################################
# integrationtests for orders:                                                         #
########################################################################################

    # (Checklist) Deze test onderhoud van alle endpoints de inputs/outputs:
    # X POST   post_orders()
    # X GET    get_orders()
    # X GET    get_order(order_id)
    # X GET    get_items_in_order(order_id)
    # X PUT    update_order(order_id, updated_order)
    # X PUT    update_items_in_order(order_id)
    # X DELETE remove_order(order_id)

def test_data_post_orders():
    # To-Do: Toevoegen van orders.
    requests.delete(f"{BASE_URL}/orders/1", headers={"API_KEY": API_KEY})
    requests.delete(f"{BASE_URL}/orders/2", headers={"API_KEY": API_KEY})

    order_1 = {"id": 1, "source_id": 33, "order_date": "2019-04-03T11:33:15Z", "request_date": "2019-04-07T11:33:15Z", "reference": "ORD00001", "reference_extra": "Bedreven arm straffen bureau.", "order_status": "Pending", "notes": "Voedsel vijf vork heel.", "shipping_notes": "Buurman betalen plaats bewolkt.", "picking_notes": "Ademen fijn volgorde scherp aardappel op leren.", "warehouse_id": 18, "ship_to": 1, "bill_to": 1, "shipment_id": 1, "total_amount": 9905.13, "total_discount": 150.77, "total_tax": 372.72, "total_surcharge": 77.6, "items": [{"item_id": "P007435", "amount": 23}]}
    order_2 = {"id": 2, "source_id": 9, "order_date": "1999-07-05T19:31:10Z", "request_date": "1999-07-09T19:31:10Z", "reference": "ORD00002", "reference_extra": "Vergelijken raak geluid beetje altijd.", "order_status": "Pending", "notes": "We hobby thee compleet wiel fijn.", "shipping_notes": "Nood provincie hier.", "picking_notes": "Borstelen dit verf suiker.", "warehouse_id": 20, "ship_to": 1, "bill_to": 1, "shipment_id": 2, "total_amount": 8484.98, "total_discount": 214.52, "total_tax": 665.09, "total_surcharge": 42.12, "items": [{"item_id": "P003790", "amount": 10}]}
    post_order_1 = requests.post(f"{BASE_URL}/orders", json=order_1, headers={"API_KEY": API_KEY})
    post_order_2 = requests.post(f"{BASE_URL}/orders", json=order_2, headers={"API_KEY": API_KEY})
    assert post_order_1.status_code == 200
    assert post_order_2.status_code == 200

def test_data_get_order():
    # To-Do: Uitlezen van order.
    order_1 = requests.get(f"{BASE_URL}/orders/1", headers={"API_KEY": API_KEY})
    assert order_1.status_code == 200

def test_data_get_orders():
    # To-Do: Uitlezen van orders.
    orders = requests.get(f"{BASE_URL}/orders", headers={"API_KEY": API_KEY})
    assert orders.status_code == 200

def get_items_in_order():
    # To-Do: Uitlezen van items in order.
    items_in_order_1 = requests.get(f"{BASE_URL}/orders/1/items", headers={"API_KEY": API_KEY})
    assert items_in_order_1.status_code == 200

def test_data_put_orders():
    # To-Do: Update de order met andere data.
    order_1 = {"id": 1, "source_id": 33, "order_date": "2019-04-03T11:33:15Z", "request_date": "2019-04-07T11:33:15Z", "reference": "ORD00001", "reference_extra": "Bedreven arm straffen bureau.", "order_status": "Delivered", "notes": "Voedsel vijf vork heel.", "shipping_notes": "Buurman betalen plaats bewolkt.", "picking_notes": "Ademen fijn volgorde scherp aardappel op leren.", "warehouse_id": 18, "ship_to": 1, "bill_to": 1, "shipment_id": 1, "total_amount": 9905.13, "total_discount": 150.77, "total_tax": 372.72, "total_surcharge": 77.6, "items": [{"item_id": "P007435", "amount": 23}]}
    order_2 = {"id": 2, "source_id": 9, "order_date": "1999-07-05T19:31:10Z", "request_date": "1999-07-09T19:31:10Z", "reference": "ORD00002", "reference_extra": "Vergelijken raak geluid beetje altijd.", "order_status": "Delivered", "notes": "We hobby thee compleet wiel fijn.", "shipping_notes": "Nood provincie hier.", "picking_notes": "Borstelen dit verf suiker.", "warehouse_id": 20, "ship_to": 1, "bill_to": 1, "shipment_id": 2, "total_amount": 8484.98, "total_discount": 214.52, "total_tax": 665.09, "total_surcharge": 42.12, "items": [{"item_id": "P003790", "amount": 10}]}
    update_order_1 = requests.put(f"{BASE_URL}/orders/1", json=order_1, headers={"API_KEY": API_KEY})
    update_order_2 = requests.put(f"{BASE_URL}/orders/2", json=order_2, headers={"API_KEY": API_KEY})
    assert update_order_1.status_code == 200
    assert update_order_2.status_code == 200

def test_data_update_items_in_order():
    # To-Do: Update de items in order met andere data.
    items_in_order_1 = [{"item_id": "P007435", "amount": 23}]
    update_items_in_order_1 = requests.put(f"{BASE_URL}/orders/1/items", json=items_in_order_1, headers={"API_KEY": API_KEY})
    assert update_items_in_order_1.status_code == 200

def test_data_delete_orders():
    # To-Do: orders verwijderen en json leegmaken.
    delete_order_1 = requests.delete(f"{BASE_URL}/orders/1", headers={"API_KEY": API_KEY})
    delete_order_2 = requests.delete(f"{BASE_URL}/orders/2", headers={"API_KEY": API_KEY})
    assert delete_order_1.status_code == 200
    assert delete_order_2.status_code == 200

########################################################################################
# integrationtests for shipments:                                                      #
########################################################################################

    # (Checklist) Deze test onderhoud van alle endpoints de inputs/outputs:
    # ✓ GET    get_shipment(shipment_id)
    # ✓ GET    get_updated_shipment(shipment_id)
    # ✓ GET    get_deleted_shipment()
    # ✓ POST   add_shipment(new_client)
    # ✓ PUT    update_shipment(shipment_id, updated_shipment)
    # ✓ DELETE remove_shipment(shipment_id)

def test_data_post_shipment():
    requests.delete(f"{BASE_URL}/shipments/1", headers={"API_KEY": API_KEY})
    new_shipment = { "id": 1,
        "order_id": 1,
        "source_id": 52,
        "order_date": "1973-01-28",
        "request_date": "1973-01-30",
        "shipment_date": "1973-02-01",
        "shipment_type": "I",
        "shipment_status": "Pending",
        "notes": "Hoog genot springen afspraak mond bus.",
        "carrier_code": "DHL",
        "carrier_description": "DHL Express",
        "service_code": "NextDay",
        "payment_type": "Automatic",
        "transfer_mode": "Ground",
        "total_package_count": 29,
        "total_package_weight": 463.0,
        "items": [
            {
                "item_id": "P010669",
                "amount": 16
            }
        ]
    }
    reponse = requests.post(f"{BASE_URL}/shipments", json=new_shipment, headers={"API_KEY": API_KEY})
    assert reponse.status_code == 200


def test_data_get_shipment():
    response = requests.get(f"{BASE_URL}/shipments/1", headers={"API_KEY": API_KEY})
    assert response.status_code == 200
    assert response.json().get('id') == 1
    
  
def test_data_put_shipment():
    updated_shipment = {
        "id": 1,
        "order_id": 1,
        "source_id": 52,
        "order_date": "1973-01-28",
        "request_date": "1973-01-30",
        "shipment_date": "1973-02-01",
        "shipment_type": "I",
        "shipment_status": "Pending",
        "notes": "Het is geleverd",
        "carrier_code": "DHL",
        "carrier_description": "DHL Express",
        "service_code": "NextDay",
        "payment_type": "Automatic",
        "transfer_mode": "Ground",
        "total_package_count": 29,
        "total_package_weight": 463.0,
        "items": [
            {
                "item_id": "P010669",
                "amount": 20
            }
        ]
    }

    response = requests.put(f"{BASE_URL}/shipments/{updated_shipment['id']}", json=updated_shipment, headers={"API_KEY": API_KEY})
    assert response.status_code == 200


def test_data_check_put_shipment():
    response = requests.get(f"{BASE_URL}/shipments/1", headers={"API_KEY": API_KEY})
    response_json = response.json()

    assert response_json["notes"] == "Het is geleverd"
    assert response_json["items"][0]["amount"] == 20


def test_data_delete_shipment():
    response = requests.delete(f"{BASE_URL}/shipments/1", headers={"API_KEY": API_KEY})
    assert response.status_code == 200


def test_data_check_delete_shipment():
    response = requests.get(f"{BASE_URL}/shipments/1", headers={"API_KEY": API_KEY})
    assert response.status_code == 404


########################################################################################
# Integration tests for suppliers:                                                      #
########################################################################################

# (Checklist) Deze test onderhoud van alle endpoints de inputs/outputs:
# ✓ GET    get_supplier(supplier_id)
# ✓ GET    get_updated_supplier(supplier_id)
# ✓ GET    get_deleted_supplier()
# ✓ POST   add_supplier(new_supplier)
# ✓ PUT    update_supplier(supplier_id, updated_supplier)
# ✓ DELETE remove_supplier(supplier_id)


def test_data_post_supplier():
    new_supplier = {
        "id": 1,
        "code": "SUP0001",
        "name": "Lee, Parks and Johnson",
        "address": "5989 Sullivan Drives",
        "address_extra": "Apt. 996",
        "city": "Port Anitaburgh",
        "zip_code": "91688",
        "province": "Illinois",
        "country": "Czech Republic",
        "contact_name": "Toni Barnett",
        "phonenumber": "363.541.7282x36825",
        "reference": "LPaJ-SUP0001"
    }
    response = requests.post(f"{BASE_URL}/suppliers", json=new_supplier, headers={"API_KEY": API_KEY})
    assert response.status_code == 200


def test_data_get_supplier():
    response = requests.get(f"{BASE_URL}/suppliers/1", headers={"API_KEY": API_KEY})
    assert response.status_code == 200
    assert response.json().get('id') == 1


def test_data_put_supplier():
    updated_supplier = {
        "id": 1,
        "code": "SUP0001",
        "name": "Lee, Parks and Johnson - Updated",
        "address": "5989 Sullivan Drives",
        "address_extra": "Apt. 996",
        "city": "Port Anitaburgh",
        "zip_code": "91688",
        "province": "Illinois",
        "country": "Czech Republic",
        "contact_name": "Toni Barnett",
        "phonenumber": "363.541.7282x36825",
        "reference": "LPaJ-SUP0001"
    }

    response = requests.put(f"{BASE_URL}/suppliers/{updated_supplier['id']}", json=updated_supplier, headers={"API_KEY": API_KEY})
    assert response.status_code == 200


def test_data_check_put_supplier():
    supplier = {
        "id": 1,
        "name": "Lee, Parks and Johnson - Updated"
    }

    response = requests.get(f"{BASE_URL}/suppliers/{supplier['id']}", headers={"API_KEY": API_KEY})
    response_data = response.json()
    assert response_data["name"] == supplier["name"]


def test_data_delete_supplier():
    response = requests.delete(f"{BASE_URL}/suppliers/1", headers={"API_KEY": API_KEY})
    assert response.status_code == 200


def test_data_check_delete_supplier():
    response = requests.get(f"{BASE_URL}/suppliers/1", headers={"API_KEY": API_KEY})
    assert response.status_code == 404

########################################################################################
# Integration tests for transfers:                                                      #
########################################################################################

# (Checklist) Deze test onderhoud van alle endpoints de inputs/outputs:
# ✓ GET    get_transfer(transfer_id)
# ✓ GET    get_updated_transfer(transfer_id)
# ✓ GET    get_deleted_transfer()
# ✓ POST   add_transfer(new_transfer)
# ✓ PUT    update_transfer(transfer_id, updated_transfer)
# ✓ DELETE remove_transfer(transfer_id)


def test_data_post_transfer():
    requests.delete(f"{BASE_URL}/transfers/1", headers={"API_KEY": API_KEY})
    new_transfer = {
        "id": 1,
        "reference": "TR00001",
        "transfer_from": None,
        "transfer_to": "9229",
        "transfer_status": "Pending",
        "items": [
            {
                "item_id": "P007435",
                "amount": 23
            }
        ]
    }

    response = requests.post(f"{BASE_URL}/transfers", json=new_transfer, headers={"API_KEY": API_KEY})
    assert response.status_code == 200


def test_data_get_transfer():
    response = requests.get(f"{BASE_URL}/transfers/1", headers={"API_KEY": API_KEY})
    assert response.status_code == 200
    assert response.json().get('id') == 1


def test_data_put_transfer():
    transfer = {
        "id": 1,
        "reference": "TR00001",
        "transfer_from": None,
        "transfer_to": "9229",
        "transfer_status": "Not Completed",
        "items": [
            {
                "item_id": "P007435",
                "amount": 20
            }
        ]
    }

    response = requests.put(f"{BASE_URL}/transfers/{transfer['id']}", json=transfer, headers={"API_KEY": API_KEY})
    assert response.status_code == 200


def test_data_check_put_transfer():
    transfer = {
        "id": 1,
        "transfer_status": "Not Completed",
        "items": [
            {
                "item_id": "P007435",
                "amount": 20
            }
        ]
    }
    
    response = requests.get(f"{BASE_URL}/transfers/{transfer['id']}", headers={"API_KEY": API_KEY})
    assert response.status_code == 200

    data = response.json()
    assert data["items"][0]["amount"] == 20


def test_data_delete_transfer():
    response = requests.delete(f"{BASE_URL}/transfers/1", headers={"API_KEY": API_KEY})
    assert response.status_code == 200


def test_data_check_delete_transfer():
    response = requests.get(f"{BASE_URL}/transfers/1", headers={"API_KEY": API_KEY})
    assert response.status_code == 404


########################################################################################
# Integration tests for warehouses:                                                      #
########################################################################################

# (Checklist) Deze test onderhoud van alle endpoints de inputs/outputs:
# ✓ GET    get_warehouse(warehouse_id)
# ✓ GET    get_updated_warehouse(warehouse_id)
# ✓ GET    get_deleted_warehouse()
# ✓ POST   add_warehouse(new_warehouse)
# ✓ PUT    update_warehouse(warehouse_id, updated_warehouse)
# ✓ DELETE remove_warehouse(warehouse_id)


def test_data_post_warehouse():
    new_warehouse = {
        "id": 1,
        "code": "YQZZNL56",
        "name": "Heemskerk cargo hub",
        "address": "Karlijndreef 281",
        "zip": "4002 AS",
        "city": "Heemskerk",
        "province": "Friesland",
        "country": "NL",
        "contact": {
            "name": "Fem Keijzer",
            "phone": "(078) 0013363",
            "email": "blamore@example.net"
        }
    }

    response = requests.post(f"{BASE_URL}/warehouses", json=new_warehouse, headers={"API_KEY": API_KEY})
    assert response.status_code == 200


def test_data_get_warehouse():
    response = requests.get(f"{BASE_URL}/warehouses/1", headers={"API_KEY": API_KEY})
    assert response.status_code == 200
    assert response.json().get('id') == 1


def test_data_put_warehouse():
    updated_warehouse = {
        "id": 1,
        "code": "YQZZNL56",
        "name": "Heemskerk cargo hub",
        "address": "Karlijndreef 281",
        "zip": "4002 AS",
        "city": "Heemskerk",
        "province": "Groningen",
        "country": "NL",
        "contact": {
            "name": "Fem Keijzer",
            "phone": "(078) 0013363",
            "email": "blamore@example.net"
        }
    }

    response = requests.put(f"{BASE_URL}/warehouses/{updated_warehouse['id']}", json=updated_warehouse,  headers={"API_KEY": API_KEY})
    assert response.status_code == 200


def test_data_check_put_warehouse():
    response = requests.get(f"{BASE_URL}/warehouses/1", headers={"API_KEY": API_KEY})
    assert response.status_code == 200

    data = response.json()
    assert data["province"] == "Groningen"


def test_data_delete_warehouse():
    response = requests.delete(f"{BASE_URL}/warehouses/1", headers={"API_KEY": API_KEY})
    assert response.status_code == 200


def test_data_check_delete_warehouse():
    response = requests.get(f"{BASE_URL}/warehouses/1", headers={"API_KEY": API_KEY})
    assert response.status_code == 404