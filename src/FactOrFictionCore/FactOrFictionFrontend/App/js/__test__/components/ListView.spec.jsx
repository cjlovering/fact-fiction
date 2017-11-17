import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import ListView from '../../components/ListView.jsx';

describe('ListView', () => {

    let state;    
    let listView;
    let selectEntry;
    let selectedEntryId;
    let loadFunc;
    let hasMore;
    
    beforeEach(() => {
        state = [{"content": "hi", "type":"OBJECTIVE", "id": "theId"}]     
        selectEntry = jest.fn();
        selectedEntryId = "abc";
        hasMore = false;
        loadFunc = jest.fn();
        listView = mount(
            <ListView
                entries={state}
                selectedEntryId={selectedEntryId}
                selectEntry={selectEntry}
                loadFunc={loadFunc}
                hasMore={hasMore}
            />);
    });

    it('ListView requires entries prop', () => {
        expect(listView.props().entries).toBeDefined();
    });

    it('ListView requires selectEntry prop', () => {
        expect(listView.props().selectEntry).toBeDefined();
    });

    it('ListView requires selectedEntryId prop', () => {
        expect(listView.props().selectedEntryId).toBeDefined();
    });

    it('ListView renders nested components == 1', () => {
        expect(listView.find('FactCard').length).toEqual(1);
    });

    it('ListView renders nested components == number of facts', () => {
        state = []
        listView = mount(
            <ListView
                entries={state}
                selectEntry={selectEntry}
                selectedEntryId={selectedEntryId}
                loadFunc={loadFunc}
                hasMore={hasMore}
            />
        );
        expect(listView.find('FactCard').length).toEqual(0);
    });

    it('ListView renders nested components == number of facts', () => {
        state = [
            {"content": "hi", "type":"OBJECTIVE"},
            {"content": "hi", "type":"OBJECTIVE"},
            {"content": "hi", "type":"OTHER"},
            {"content": "hi", "type":"SUBJECTIVE"}
        ]
        listView = mount(
            <ListView
                entries={state}
                selectEntry={selectEntry}
                selectedEntryId={selectedEntryId}
                loadFunc={loadFunc}
                hasMore={hasMore}
            />
        );
        expect(listView.find('FactCard').length).toEqual(2);
    });
});