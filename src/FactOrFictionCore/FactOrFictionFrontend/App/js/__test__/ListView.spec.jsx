import React from 'react';
import expect from 'expect';
import { mount } from 'enzyme';
import ListView from '../components/ListView.jsx';

describe('ListView', () => {

    let state;    
    let listView;
    let selectEntry;
    let selectedEntryId;
    
    beforeEach(() => {
        state = [{"content": "hi", "type":"OBJECTIVE", "id": "theId"}]     
        selectEntry = jest.fn();
        selectedEntryId = "abc";
        listView = mount(
            <ListView
                entries={state}
                selectedEntryId={selectedEntryId}
                selectEntry={selectEntry}
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
            />
        );
        expect(listView.find('FactCard').length).toEqual(2);
    });
});